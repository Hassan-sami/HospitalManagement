using AutoMapper;
using Hospital.BLL.Helpers;
using Hospital.BLL.ModelVM;
using Hospital.BLL.Services.Abstraction;
using Hospital.DAL.Entities;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.DotNet.Scaffolding.Shared;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
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
                    var assignRoleResult = await userManager.AddToRoleAsync(user,Role.Patient.ToString());
                    if(!assignRoleResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "error try again");

                        await userManager.DeleteAsync(user);

                        return View(registerViewModel);
                    }
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
        public IActionResult LogIn(string? returnUrl)
        {
            var model = new LogInViewModel()
            {
                ReturnUrl = returnUrl
            };
            return View(model);
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
                    if(viewModel.ReturnUrl != null)
                        return LocalRedirect(viewModel.ReturnUrl);
                    return RedirectToAction("Index","Home");
                }
                ModelState.AddModelError("", " Email or passowrd wrong try again");

            }

            return View("LogIn", viewModel);

        }
        [HttpGet]
        public async  Task<IActionResult> GoogleAuth(string? returnUrl)
        {
             var provider = (await signInManager.GetExternalAuthenticationSchemesAsync()).FirstOrDefault(x => x.Name == GoogleDefaults.DisplayName);
            return RedirectToAction("ExtrenalAuth", new {returnUrl = returnUrl, provider = provider.Name });
        }
        [HttpGet]
        public IActionResult ExtrenalAuth(string? returnUrl, string provider)
            {
            
            var redirectUrl = Url.Action(action: "ExternalLoginCallback", controller: "Account", values: new { ReturnUrl = returnUrl }, Request.Scheme);
            //var redirectUrl = Url.Action(action: "LogIn", controller: "Account");
            // Configure the redirect URL, provider and other properties
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            //This will redirect the user to the external provider's login page
            return new ChallengeResult(provider,properties);
        }


        [HttpGet]

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl, string? remoteError)
        {
            LogInViewModel loginViewModel = new LogInViewModel
            {
                ReturnUrl = returnUrl,

            };
            if (remoteError != null)
            {

                ModelState.AddModelError(string.Empty, $"Error Form google {remoteError}");
                return RedirectToAction("LogIn", loginViewModel);
            }
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");
                return View("LogIn", loginViewModel);
            }
            //    var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
            //info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl ?? "/Home/Index");
            }
            else
            {
                // Get the email claim value
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    // Create a new user without password if we do not have a user already
                    var user = await userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new Patient
                        {
                            UserName = new MailAddress(info.Principal.FindFirstValue(ClaimTypes.Email)).User,
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)??"Notfound name",
                            LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)?? "NotFound Name",
                            PhoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone)

                        };
                        //This will create a new user into the AspNetUsers table without password
                        await userManager.CreateAsync(user);
                        await userManager.AddToRoleAsync(user,Role.Patient.ToString());
                    }
                    // Add a login (i.e., insert a row for the user in AspNetUserLogins table)
                    await userManager.AddLoginAsync(user, info);
                    //Then Signin the User
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl??"/Home/Index");
                }

                ModelState.AddModelError(string.Empty, "Error loading external login information.");
                return View("LogIn", loginViewModel);

            }
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            if (ModelState.IsValid) 
            {
                var user = await userManager.FindByEmailAsync(forgetPasswordVM.Email);
                if(user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var code = await userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callBackAction = Url.Action(new Microsoft.AspNetCore.Mvc.Routing.UrlActionContext()
                    {
                        Action = "ResetPassword",
                        Controller = "Account",
                        Values = new { code =  code,email = forgetPasswordVM.Email },
                        //Host = Request.Host.Value,
                        Protocol = Request.Scheme,


                    });
                    bool sent = await sender.send(user.Email, "reset  Hospital password",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callBackAction)}'>clicking here</a>.");
                    return RedirectToAction("ResetConfirmation", new {email = user.Email,code = code});
                }
                ModelState.AddModelError(string.Empty, "not found user");
            }
            return View(forgetPasswordVM);
        }
        
        public IActionResult ResetConfirmation(string? email,string? code)
        {
            ViewBag.Email = email;
            ViewBag.code = code;
            return View();
        }

        [HttpGet]
        public async  Task<IActionResult> ResendEmail(string email,string code)
        {
            var callBackAction = Url.Action(new Microsoft.AspNetCore.Mvc.Routing.UrlActionContext()
            {
                Action = "ResetPassword",
                Controller = "Account",
                Values = new { code = code, email = email},
                //Host = Request.Host.Value,
                Protocol = Request.Scheme,


            });
            bool sent = await sender.send(email, "reset  Hospital password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callBackAction)}'>clicking here</a>.");

            return RedirectToAction("ResetConfirmation", new { code = code, email = email });
        }

        [HttpGet]
        public  IActionResult ResetPassword(string? code,string? email)
        {
            if(code == null || email == null)
                return BadRequest("A code must be supplied for password reset.");
            ResetPasswordVM resetPasswordVM = new ResetPasswordVM()
            {
                Code = code,
                Email = email
            };

            return View(resetPasswordVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(resetPasswordVM.Email);
                if (user != null)
                {
                    resetPasswordVM.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordVM.Code));
                    var result = await userManager.ResetPasswordAsync(user, resetPasswordVM.Code, resetPasswordVM.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("LogIn");
                    }
                    else
                        ModelState.AddModelError(string.Empty, "fail to reset click resent email");
                }
                else
                    return RedirectToAction("ForgetPassword", "Account");
            }

            return View(resetPasswordVM);
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            if(User?.Identity?.IsAuthenticated?? false)
            {
                await signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "Home");
        }


    }
}
