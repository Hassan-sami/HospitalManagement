using AutoMapper;
using Hospital.BLL.ModelVM;
using Hospital.BLL.Services.Abstraction;
using Hospital.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(IMapper mapper, UserManager<ApplicationUser> userManager,
            IPatientService patientService, IAppointmentService appointmentService,
            IMedicalRecordService medicalRecordService, ILogger<PatientController> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _patientService = patientService;
            _appointmentService = appointmentService;
            _medicalRecordService = medicalRecordService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var patient = await _patientService.GetPatientById(userId);
            if (patient == null)
            {
                _logger.LogWarning("Patient not found for user {UserId}", userId);
                return NotFound();
            }

            var patientVm = _mapper.Map<PatientVm>(patient);
            patientVm.Appointments = await _appointmentService.GetAppointmentsByPatientId(userId);
            patientVm.MedicalRecords = await _medicalRecordService.GetMedicalRecordsByPatientId(userId);

            return View(patientVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookAppointment(AppointmentVm appointmentVm)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                var appointment = new Appointment
                {
                    PatientID = userId,
                    AppointmentDate = appointmentVm.AppointmentDate,
                    Status = appointmentVm.Status,
                    Notes = appointmentVm.Notes,
                    DoctorID = appointmentVm.DoctorId
                };

                var result = await _appointmentService.CreateAppointment(appointment);
                if (result)
                {
                    _logger.LogInformation("Appointment successfully created for user {UserId}", userId);
                    return RedirectToAction("Index");
                }

                _logger.LogError("Failed to create appointment for user {UserId}", userId);
                ModelState.AddModelError(string.Empty, "Failed to book appointment. Try again.");
            }

            return View(appointmentVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAppointment(AppointmentVm appointmentVm)
        {
            if (ModelState.IsValid)
            {
                var result = await _appointmentService.UpdateAppointment(appointmentVm);
                if (result)
                {
                    _logger.LogInformation("Appointment successfully updated for user {UserId}", _userManager.GetUserId(User));
                    return RedirectToAction("Index");
                }

                _logger.LogError("Failed to update appointment for user {UserId}", _userManager.GetUserId(User));
                ModelState.AddModelError(string.Empty, "Failed to update appointment. Try again.");
            }

            return View(appointmentVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result = await _appointmentService.DeleteAppointment(id);
            if (result)
            {
                _logger.LogInformation("Appointment successfully deleted for user {UserId}", _userManager.GetUserId(User));
                return RedirectToAction("Index");
            }

            _logger.LogError("Failed to delete appointment for user {UserId}", _userManager.GetUserId(User));
            ModelState.AddModelError(string.Empty, "Failed to delete appointment. Try again.");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ViewMedicalRecord(int id)
        {
            var medicalRecord = await _medicalRecordService.GetMedicalRecordById(id);
            if (medicalRecord == null)
            {
                _logger.LogWarning("Medical record not found for ID {MedicalRecordId}", id);
                return NotFound();
            }

            return View(medicalRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMedicalRecord(MedicalRecordVm medicalRecordVm)
        {
            if (ModelState.IsValid)
            {
                var medicalRecord = _mapper.Map<MedicalRecord>(medicalRecordVm);
                var result = await _medicalRecordService.CreateMedicalRecord(medicalRecord);
                if (result)
                {
                    _logger.LogInformation("Medical record successfully created for patient {PatientId}", medicalRecord.PatientID);
                    return RedirectToAction("Index");
                }

                _logger.LogError("Failed to create medical record for patient {PatientId}", medicalRecord.PatientID);
                ModelState.AddModelError(string.Empty, "Failed to create medical record. Try again.");
            }

            return View(medicalRecordVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMedicalRecord(MedicalRecordVm medicalRecordVm)
        {
            if (ModelState.IsValid)
            {
                var result = await _medicalRecordService.UpdateMedicalRecord(medicalRecordVm);
                if (result)
                {
                    _logger.LogInformation("Medical record successfully updated for patient {PatientId}", medicalRecordVm.PatientId);
                    return RedirectToAction("Index");
                }

                _logger.LogError("Failed to update medical record for patient {PatientId}", medicalRecordVm.PatientId);
                ModelState.AddModelError(string.Empty, "Failed to update medical record. Try again.");
            }

            return View(medicalRecordVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            var result = await _medicalRecordService.DeleteMedicalRecord(id);
            if (result)
            {
                _logger.LogInformation("Medical record successfully deleted for ID {MedicalRecordId}", id);
                return RedirectToAction("Index");
            }

            _logger.LogError("Failed to delete medical record for ID {MedicalRecordId}", id);
            ModelState.AddModelError(string.Empty, "Failed to delete medical record. Try again.");
            return RedirectToAction("Index");
        }
    }
}
