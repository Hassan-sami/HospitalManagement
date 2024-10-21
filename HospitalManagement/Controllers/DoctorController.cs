using AutoMapper;
using Hangfire;
using Hospital.BLL.ModelVM;
using Hospital.BLL.Services.Abstraction;
using Hospital.BLL.Services.Implementation;
using Hospital.DAL.Entities;
using Hospital.DAL.Entities.OwnedTypes;
using Hospital.DAL.Repository.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace HospitalManagement.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly IRecurringJobManager recurringJobManager;
        private readonly IScheduleService scheduleService;
        private readonly IEmailSender sender;
        private readonly IPatientService patientService;
        private readonly ImedicalRecordService medicalRecordService;
        private readonly IMapper mapper;

        public DoctorController(IAppointmentService appointmentService,
            IRecurringJobManager recurringJobManager,
            IScheduleService scheduleService, 
            IEmailSender sender,IPatientService patientService,
            ImedicalRecordService medicalRecordService, IMapper mapper)
        {
            this.appointmentService = appointmentService;
            this.recurringJobManager = recurringJobManager;
            this.scheduleService = scheduleService;
            this.sender = sender;
            this.patientService = patientService;
            this.medicalRecordService = medicalRecordService;
            this.mapper = mapper;
        }

        public IActionResult GetAppointments()
        {
            recurringJobManager.AddOrUpdate("DailyNotApproved", () =>
              appointmentService.UpdateAppointmentStatus(AppointStatus.NotApproved)

            , Cron.Daily);   
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var  appointments = appointmentService.GetAppointments(appoint => appoint.DoctorID == userId && appoint.AppointmentDate.Date > DateTime.Now.Date);
            return View(appointments);
        }
        [HttpGet]
        public async Task<IActionResult> EditAppointment(int id)
        {
            var appointment = await appointmentService.GetAppointmentById(id);
            DoctorAppointmentApprovalVm doctorAppointmentApprovalVm = new DoctorAppointmentApprovalVm()
            {
                Status = (int)appointment.Status,
                Id = appointment.AppointmentID

            };
            return View(doctorAppointmentApprovalVm);
        }

        [HttpPost]
        
        public async Task<IActionResult> EditAppointment(DoctorAppointmentApprovalVm Model)
        {
            if (ModelState.IsValid && Model.Id  != 0 && Model.Status != 0)
            {
                var appointment = await appointmentService.GetAppointmentById(Model.Id);

                var docId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Model.Status == (int)AppointStatus.Approved)
                {
                    var schedules =  scheduleService.GetDoctorSchedulesById(docId);
                    if (schedules != null)
                    {
                        foreach (var sch in schedules)
                        {
                            if (sch.Day == appointment.AppointmentDate.DayOfWeek && appointmentService.GetAppointments(app =>
                                app.AppointmentDate == appointment.AppointmentDate).Count() < 50)
                            {
                                appointment.Schedule = sch;
                                appointment.ScheduleId = sch.Id;
                                appointment.Status = (AppointStatus)Model.Status;
                                await appointmentService.UpdateAppointment(appointment);
                                await sender.send(appointment.Patient.Email, "Your appointment Approval",
                                    $"your appointment is accepted Date {appointment.AppointmentDate}  " +
                                    $"\nDay {appointment.AppointmentDate} " +
                                    $"\n {sch.Shift.ShiftType} From {sch.Shift.StartTIme} to {sch.Shift.EndTIme}");
                                return RedirectToAction("GetAppointments");
                            }
                        }
                        
                        ModelState.AddModelError(string.Empty,"can't add this appointment");
                    }
                    else
                        ModelState.AddModelError(string.Empty, "there no shift in this date");
                }
                else
                {
                    appointment.Status = (AppointStatus)Model.Status;
                    await appointmentService.UpdateAppointment(appointment);
                    return RedirectToAction("GetAppointments");
                }
                
            }
            Model.Id = Model.Id;
            return View(Model);

        }
        public IActionResult GetMedicalRecords()
        {
            var model = medicalRecordService.
                GetDoctorMedicalRecords(p => p.DoctorID == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .OrderByDescending(m => m.RecordDate).Take(15).ToList();
            return View(model);
        }

        public IActionResult AddMedicalRecord()
        {
            var CreateMedicalRecord = new AddMedicalRecordByDoctorVm()
            {
                patients = patientService.GetAllPatients(),

            };

            return View(CreateMedicalRecord);
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicalRecord(AddMedicalRecordByDoctorVm addmedicalRecordVM)
        {
            if (ModelState.IsValid)
            {
                var medicalRecord = mapper.Map<MedicalRecord>(addmedicalRecordVM);
                medicalRecord.DoctorID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                if (await medicalRecordService.AddMedicalRecord(medicalRecord))
                {
                    return RedirectToAction("GetMedicalRecords");
                }
                ModelState.AddModelError(string.Empty, "can not add this record try again ");
            }

            addmedicalRecordVM.patients = patientService.GetAllPatients();
            
            return View("AddMedicalRecord", addmedicalRecordVM);
        }
        [HttpGet]

        public IActionResult GetRecords(int num)
        {
            var model = medicalRecordService.
                GetDoctorMedicalRecords(p => p.DoctorID == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Select( m => 
                
                     new MedicalRecordVMByAj
                    {
                         Diagnosis = m.Diagnosis,
                         Treatment = m.Treatment,
                        RecordDate  = m.RecordDate.Date,
                         FullName = m.Patient?.FirstName+  m.Patient?.LastName,
                         Email = m.Patient?.Email,
                         Phone = m.Patient?.PhoneNumber
                    }
                
                ).OrderByDescending(m => m.RecordDate).Take(num).ToArray();
            return Ok(model);
        }

        public IActionResult GetAppoints(int num)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointments = appointmentService
                .GetAppointments(appoint => appoint.DoctorID == userId 
                && appoint.AppointmentDate.Date > DateTime.Now.Date).
                Select(ap => new AppointVmByAj
                {
                    FullName = ap.Patient?.FirstName + " " + ap.Patient.LastName,
                    Date = ap.AppointmentDate.Date,
                    Notes = ap.Notes,
                    AppointId = ap.Status == AppointStatus.Approved ? ap.AppointmentID.ToString() : ap.Status.ToString(),
                    Status = ap.Status.ToString(),
                    Id = ap.AppointmentID


                })
                .OrderByDescending(a => a.Date).Take(num).ToArray();
            return Ok(appointments);
        }
    }
}
