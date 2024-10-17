using AutoMapper;
using Hospital.BLL.Services.Abstraction;
using Hospital.BLL.Services.Implementation;
using Hospital.DAL.Entities;
using Hospital.DAL.Repository.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagement.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private readonly IDoctorService doctorService;
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly IAppointmentService appointmentService;

        public PatientController(IDoctorService doctorService, IPatientService patientService
            , IMapper mapper, IAppointmentService appointmentService)
        {
            this.doctorService = doctorService;
            this.patientService = patientService;
            this.mapper = mapper;
            this.appointmentService = appointmentService;
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
                if (model.AppointmentDate >= DateTime.Now.Date)
                {
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var patient = await patientService.GetPatientById(userid);
                    var appontMent = mapper.Map<Appointment>(model);
                    appontMent.Status = "Pending";
                    appontMent.PatientID = userid;
                    appontMent.Patient = patient;
                    var added = await appointmentService.AddAppointment(appontMent);
                    if (added)
                        return RedirectToAction("ListAppointments");
                    else
                    {
                        ModelState.AddModelError(string.Empty, "fail to save this appointment !!");
                        return View(model);
                    }
                }
                ModelState.AddModelError(string.Empty, "Please Enter the Date greater than today or equal!!");
            }


            return View(model);

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


    }
}
