using AutoMapper;
using Hospital.BLL.Services.Abstraction;
using Hospital.BLL.Services.Implementation;
using Hospital.DAL.Entities;
using Hospital.DAL.Entities.OwnedTypes;
using Hospital.DAL.Repository.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Win32;
using System.Security.Claims;
using System.Text;

namespace HospitalManagement.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private readonly IDoctorService doctorService;
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly IAppointmentService appointmentService;
        private readonly ImedicalRecordService medicalRecordService;
        private readonly IScheduleService scheduleService;

        public PatientController(IDoctorService doctorService, IPatientService patientService
            , IMapper mapper, IAppointmentService appointmentService,
            ImedicalRecordService medicalRecordService,IScheduleService scheduleService)
        {
            this.doctorService = doctorService;
            this.patientService = patientService;
            this.mapper = mapper;
            this.appointmentService = appointmentService;
            this.medicalRecordService = medicalRecordService;
            this.scheduleService = scheduleService;
        }

        [HttpGet]
        public ActionResult AddAppointment()
        {
            var appointmentVm = new AppointmentVm
            {
                Doctors = doctorService.GetAllDoctors()

            };
            return View(appointmentVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAppointment(AppointmentVm model)
        {
            model.Doctors = doctorService.GetAllDoctors();
            if (ModelState.IsValid)
            {
                if (model.AppointmentDate > DateTime.Now.Date)
                {
                    if(AppointmentHandler(model.AppointmentDate,model.AppointmentDate.DayOfWeek,model.DoctorId) == 0)
                    {
                        var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var patient = await patientService.GetPatientById(userid);
                        var appontMent = mapper.Map<Appointment>(model);
                        appontMent.Status = AppointStatus.Pending;
                        appontMent.PatientID = userid;
                        appontMent.Patient = patient;
                        var added = await appointmentService.AddAppointment(appontMent);
                        if (added)
                        {
                            return RedirectToAction("ListAppointments");
                        }    
                        else
                        {
                            ModelState.AddModelError(string.Empty, "fail to save this appointment !!");
                            return View(model);
                        }
                    }
                    else if(AppointmentHandler(model.AppointmentDate, model.AppointmentDate.DayOfWeek, model.DoctorId) == 1)
                    {
                        ModelState.AddModelError(string.Empty, "Full! this date appointments booked");
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "there is no shift for this doctor in this date.\n " +
                            "please check docor schedule");
                        return View(model);
                    }
                }
                ModelState.AddModelError(string.Empty, "Please Enter the Date greater than today !!");
            }


            return View(model);

        }
        private int AppointmentHandler(DateTime date,DayOfWeek day, string docId)
        {
            var schedules = scheduleService.GetDoctorSchedulesById(docId);
            foreach (var schedule in schedules)
            {
                if(schedule.Day == day)
                {
                    
                    if (appointmentService.GetAppointments(app => app.Doctor.Id == docId &&
                    app.AppointmentDate == date).Count() < 50)
                    {
                        return 0;
                    }
                    else
                        return 1;

                }
                
            }
            return 2;
        }
        public async Task<IActionResult> ListAppointments()
        {
            string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var patient = await patientService.GetPatientById(userid);
            var appointment = appointmentService.GetAppointments(a => a.PatientID == userid).ToList();
            return View(appointment);
        }
        public async Task<ActionResult> AppointmentDetails(int? id)
        {
            if (id == null)
                return RedirectToAction("ListAppointments");
            var appointment = await appointmentService.GetAppointmentById(id.Value);
            var appontmentvm = mapper.Map<AppointmentVm>(appointment);
            appontmentvm.Id = id;
            appontmentvm.Doctors = doctorService.GetAllDoctors();
            return View(appontmentvm);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAppointment(int id)
        {
            if(await appointmentService.DeleteAppointment(id))
            {
                return Ok(new { redirect = "/Patient/ListAppointments" });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "can't delete this appointment");
                var appointment = await appointmentService.GetAppointmentById(id);
                var appontmentvm = mapper.Map<AppointmentVm>(appointment);
                appontmentvm.Id = id;
                appontmentvm.Doctors = doctorService.GetAllDoctors();
                return View(appontmentvm);
            }
            
        }

        public async Task<IActionResult> EditAppointment(AppointmentVm model)
        {
            model.Doctors = doctorService.GetAllDoctors();
            if (ModelState.IsValid)
            {
                if (model.AppointmentDate >= DateTime.Now.Date)
                {

                    var appoint =  await appointmentService.GetAppointmentById(model.Id.Value);
                    appoint.AppointmentDate = model.AppointmentDate;
                    appoint.Notes = model.Notes;
                    appoint.DoctorID = model.DoctorId;
                    appoint.Doctor = await doctorService.DoctorByIdAsync(model.DoctorId);
                    var updated = await appointmentService.UpdateAppointment(appoint);
                    if (updated)
                        return RedirectToAction("ListAppointments");
                    else
                    {
                        ModelState.AddModelError(string.Empty, "fail to save this update !!");
                        return View(model);
                    }
                }
                ModelState.AddModelError(string.Empty, "Please Enter the Date greater than today or equal!!");
            }
            return View(model);
        }

        public IActionResult GetMedicalRecords()
        {
            
            return View(medicalRecordService.
                GetMedicalRecordsWithPatientAndDoctor().Where(m => m.PatientID == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderBy(med => med.RecordDate));        }
        public async Task<IActionResult> GetDoctorDetails(string? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");
            var doc = await doctorService.GetDoctorAndSchedulesById(id);
            return View(doc);
        }


    }
}
