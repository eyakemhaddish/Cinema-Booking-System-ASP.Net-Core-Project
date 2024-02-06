using FromScratch_.net_5.Models;
using FromScratch_.net_5.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FromScratch_.net_5.Controller3
{

    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
           
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
               var result = await userManager.CreateAsync(user, model.Password);
            

            //to check if user is successfully created
            //if

            if(result.Succeeded)
                {
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Super Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    //We want session cookie so isPersistent: false
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("home", "home");
                }
                //if not succesfult to create user,
                //run through the errors
                foreach(var error in result.Errors)
                {
                ModelState.AddModelError("", error.Description);

                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
           await signInManager.SignOutAsync();
            return RedirectToAction("home", "home");
        }

        [HttpGet]
        [AllowAnonymous ]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel { 
            
                ReturnUrl = returnUrl,
                ExternalLogins =  (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            
            
            };           
            
            return View(model);

        }

        [HttpPost]
        [AllowAnonymous]
        public Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });

            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Task.FromResult<IActionResult>(new ChallengeResult(provider, properties));

        }

        [AllowAnonymous]
        public async Task<IActionResult>
            ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel
            {

                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login", loginViewModel);
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, $"Error loading external login information. ");

                return View("Login", loginViewModel);

            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await userManager.CreateAsync(user);
                    }

                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);

                }

            }
            return View(loginViewModel);



        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password,
                    model.RememberMe, false);
                  
                if (result.Succeeded)
                {  
                    if(!string.IsNullOrEmpty(returnUrl))
                    {
                        //Local redirect better than only redirect for security (Open redirect attacks) purpose
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("home", "home");
                }
               

                
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

                
            }
            return View(model);
        }
        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model, Logger logger)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await userManager.FindByEmailAsync(model.Email);
        //        if(user != null)
        //        {
        //            var token= await userManager.GeneratePasswordResetTokenAsync(user);

        //            var passwordResetLink = Url.Action("ResetPassword", "Account", 
        //                new {email = model.Email, token = token}, Request.Scheme);
        //            logger.Log(LogLevel.Warning, passwordResetLink);
        //        }
        //    }
        //    return View();
        //}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if(user == null)
                {
                    return RedirectToAction("Login");
                }
                var result = await userManager.ChangePasswordAsync(user, 
                    model.Currentpassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }
            return View(model);
        }
    }
}
