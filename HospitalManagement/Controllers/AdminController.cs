using AutoMapper;
using Hospital.BLL.Helpers;
using Hospital.BLL.ModelVM;
using Hospital.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using NuGet.Protocol.Plugins;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Text;
using Hospital.BLL.Services.Abstraction;

namespace HospitalManagement.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMapper mapper;
        private readonly IEmailSender sender;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;
        private readonly ISepcializationService sepcializationService;
        private readonly IDoctorService doctorService;
        private readonly IPatientService patientService;

        public AdminController(IMapper mapper, IEmailSender sender,
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger, ISepcializationService sepcializationService, IDoctorService doctorService,IPatientService patientService)
        {
            this.mapper = mapper;
            this.sender = sender;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.sepcializationService = sepcializationService;
            this.doctorService = doctorService;
            this.patientService = patientService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateDoctor()
        {
            CreateDoctorViewModel createDoctorViewModel = new CreateDoctorViewModel()
            {
                Specializations = sepcializationService.GetSpecializations()
            };

            return View(createDoctorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDoctor(CreateDoctorViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = mapper.Map<Doctor>(registerViewModel);
                user.Specialization = await sepcializationService.GetSpecialization(registerViewModel.SpecializationId);
                user.UserName = new MailAddress(registerViewModel.Email).User;
                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    logger.LogInformation("created doctor in db");
                    var assignRoleResult = await userManager.AddToRoleAsync(user, Role.Doctor.ToString());
                    if (!assignRoleResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "error try again");

                        await userManager.DeleteAsync(user);

                        return View(registerViewModel);
                    }
                    var id = await userManager.GetUserIdAsync(user);

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmresult =  await userManager.ConfirmEmailAsync(user, code);
                    if (!confirmresult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "error try again");

                        await userManager.DeleteAsync(user);

                        return View(registerViewModel);
                    }
                    bool sent = await sender.send(user.Email, "Confrim Hospital Registerion",
                        $"this is your password <p>{registerViewModel.Password}</p>");
                    if (!sent)
                    {
                        ModelState.AddModelError(string.Empty, "Fail to sent the email try again");

                        await userManager.DeleteAsync(user);

                        return View(registerViewModel);
                    }
                    return RedirectToAction("Registered");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(registerViewModel);
        }


        public IActionResult ListDoctors()
        {
            var result = doctorService.GetDoctorVms();

            return View(result);
        }

        public async Task<IActionResult> getDoctorDetails(string id)
        {
            var doctor = await doctorService.DoctorByIdAsync(id);
            var result = mapper.Map<DoctorVm>(doctor);
            result.specializations = sepcializationService.GetSpecializations();
            return View(result);
        }
        [HttpDelete]
        public  async Task<IActionResult> DeleteDoctorAjax(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("this doctor does not exist");
            }
            if(!(await userManager.DeleteAsync(user)).Succeeded)
            {
                return BadRequest("can not delte this doctor");
            }
            //return RedirectToAction("ListDoctors");
            return Ok(new { redirect = "/Admin/ListDoctors" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDoctor(string id, DoctorVm doctorVm)
        {
            if (ModelState.IsValid)
            {
                doctorVm.Id = id;
                var doc = await doctorService.DoctorByIdAsync(id);
                doc.FirstName =doctorVm.FirstName;
                doc.LastName =doctorVm.LastName;
                doc.Salary = doctorVm.Salary;
                doc.Specialization = await sepcializationService.GetSpecialization(doctorVm.SpecializationId);
                
                var result = await userManager.UpdateAsync(doc);
                if (result.Succeeded)
                {
                    return RedirectToAction("getDoctorDetails", new { id = id });
                }
                ModelState.AddModelError(string.Empty, "can not edit this doc try again");
            }

            return View("getDoctorDetails",doctorVm);
        }

        public IActionResult GetPatients()
        {
            var patients = patientService.GetAllPatients().Select( p=> new PatientVm()
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                Phone = p.PhoneNumber,
                Id= p.Id
            }
            ).ToList();
            
            return View(patients);
        }
        public async  Task<IActionResult> EditPatient(EditPatientVm vm)
        {
            if (ModelState.IsValid)
            {
                var patient = await patientService.GetPatientById(vm.Id);
                if (patient != null)
                {
                    patient.FirstName = vm.FirstName;
                    patient.LastName = vm.LastName;
                    patient.Email = vm.Email;
                    patient.PhoneNumber = vm.PhoneNumber;
                    patient.Gender = vm.Gender;
                    patient.DateOfBirth = vm.DateOfBirth;
                    var result = await userManager.UpdateAsync(patient);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetPatients");
                    }
                    ModelState.AddModelError(string.Empty, "can not update this patient");
                }
                else
                    ModelState.AddModelError(string.Empty, "this patient does not exist");

            }
            return View(vm);

        }

       
        public async Task<IActionResult> GetPatientDetails(string id)
        {
            var patient = await patientService.GetPatientById(id);
            var patientVm = mapper.Map<EditPatientVm>(patient); 
            return View(patientVm);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePatient(string email)
        {
            var patient = await patientService.GetPatient(p => p.Email == email);
            if (patient == null)
            {
                return NotFound("there is no patient with that email");
            }
            var result = await userManager.DeleteAsync(patient);
            if (!result.Succeeded)
            {
                return BadRequest("can not delete this patient");
            }

            //return RedirectToAction("GetPatients");
            return Ok(new { redirect = "/Admin/GetPatients" });
        }

        





    }
}
