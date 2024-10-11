using AutoMapper;
using Hospital.BLL.ModelVM;
using Hospital.BLL.Services.Abstraction;
using Hospital.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HospitalManagement.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly IMapper mapper;
        private readonly IEmailSender sender;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(IMapper mapper, IEmailSender sender, 
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger) 
        {
            this.mapper = mapper;
            this.sender = sender;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            RegisterViewModel vm = new RegisterViewModel()
            {
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {

            if (ModelState.IsValid) 
            {
                var user = mapper.Map<Patient>(registerViewModel);
                user.UserName = new MailAddress(registerViewModel.Email).User;
                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    logger.LogInformation("created user in db");
                    var id = await userManager.GetUserIdAsync(user);
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callBackAction = Url.Action(new Microsoft.AspNetCore.Mvc.Routing.UrlActionContext()
                    {
                        Action = "ConfirmEmail",
                        Controller = "Account",
                        Values = new { id = id, code =code },
                        //Host = Request.Host.Value,
                        Protocol = Request.Scheme,
                        

                    });
                    bool sent =  await sender.send(user.Email, "Confrim Hospital Registerion",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callBackAction)}'>clicking here</a>.");
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

        [HttpGet]
        public IActionResult Registered()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string? id, string? code)
        {
            if (id == null || code == null)
            {
                return RedirectToAction("Index","Home");
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{id}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return NotFound($"Unable to confrim '{id}'.");
            }

            return RedirectToAction("LogIn");

        }
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(new MailAddress(viewModel.Email).User,
                    viewModel.Password, viewModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    logger.LogInformation("User logged in.");
                    return RedirectToAction("Index","Home");
                }
                ModelState.AddModelError("", " Email or passowrd wrong try again");

            }

            return View("LogIn", viewModel);

        }


    }
}
