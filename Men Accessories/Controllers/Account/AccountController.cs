using Men_Accessories.Models;
using Men_Accessories.Utilities;
using Men_Accessories.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace Men_Accessories.Controllers.Account
{
    public class AccountController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerview)
        {
            if (ModelState.IsValid)//server side validation
            {
              
                //mapping from registerview to applicationUser
                ApplicationUser appuser = new ApplicationUser()
                {
                    UserName = registerview.userName,
                    firstName = registerview.firstName,
                    lastName = registerview.lastName,
                    Email = registerview.Email,
                    isAgree = registerview.isAgree,
                };
                var result = userManager.CreateAsync(appuser, registerview.Password).Result;
                if (result.Succeeded)
                {
                    // Generate email confirmation token
                    var token = userManager.GenerateEmailConfirmationTokenAsync(appuser).Result;

                    var confirmEmailURL = Url.Action("ConfirmEmail", "Account",
                        new { email = appuser.Email, token }, Request.Scheme);

                    var email = new Email()
                    {
                        To = registerview.Email,
                        Subject = "Confirm Your Email",
                        Body = $@"
    <div style='font-family:Arial;padding:30px;background:#f4f4f4'>
        
        <div style='max-width:600px;margin:auto;background:white;
                    border-radius:10px;padding:40px;text-align:center'>

            <h1 style='color:#4f46e5'>Welcome</h1>

            <p style='font-size:16px;color:#555'>
                Thank you for registering.
            </p>

            <p style='font-size:16px;color:#555'>
                Please confirm your email address.
            </p>

            <a href='{confirmEmailURL}'
               style='display:inline-block;
                      margin-top:20px;
                      padding:15px 30px;
                      background:#4f46e5;
                      color:white;
                      text-decoration:none;
                      border-radius:8px;
                      font-weight:bold'>
                Confirm Email
            </a>

        </div>

    </div>"
                    };

                    EmailSettings.SendEmail(email, _configuration);

                    return RedirectToAction("CheckYourInBox");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    //to make register again
                    return View(registerview);
                }

            }
            return View(registerview);

        }
        #endregion

        [HttpGet]
        public IActionResult ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Invalid email confirmation link");
                return View("Error");
            }

            var user = userManager.FindByEmailAsync(email).Result;
            if (user is null)
            {
                ModelState.AddModelError("", "User not found");
                return View("Error");
            }

            var result = userManager.ConfirmEmailAsync(user, token).Result;
            if (result.Succeeded)
                return View("ConfirmEmailSuccess"); // show success page

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View("Error");
        }

        #region Login
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Home");
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginView, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View(loginView);
            var user = userManager.FindByEmailAsync(loginView.Email).Result;
            if (user is not null)
            {
                var hasPassword = userManager.HasPasswordAsync(user).Result;

                if (!hasPassword)
                {
                    ModelState.AddModelError("", "This account uses external login (Google/Facebook). Please login using it.");
                    return View(loginView);
                }

                var result = signInManager.PasswordSignInAsync(user, loginView.Password, loginView.RememberMe, false).Result;
                if (result.Succeeded)
                    //return RedirectToLocal(returnUrl);
                    return RedirectToAction("Index", "Home");
                if (result.IsNotAllowed)
                    ModelState.AddModelError("", "Your Account Is Not Allowed to login");
                if (result.IsLockedOut)
                    ModelState.AddModelError("", "Your Account Is Locked Out");
                else
                    ModelState.AddModelError("", "Invalid Email or Password");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Login");
            }
            return View(loginView);

        }

        #endregion

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        #region Forget Password
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendResetPasswordLink(ForgetPasswordViewModel forgetPasswordView)
        {
            if (ModelState.IsValid)
            {
                //check user by email
                var user = userManager.FindByEmailAsync(forgetPasswordView.Email).Result;
                if (user is not null)
                {
                    var token = userManager.GeneratePasswordResetTokenAsync(user).Result;

                    var resetPasswordURL = Url.Action("ResetPassword", "Account", new { email = forgetPasswordView.Email, token }, Request.Scheme);
                    //create Email
                    var email = new Email()
                    {
                        To = forgetPasswordView.Email,

                        Subject = "Reset Password",

                        Body = $@"
    <div style='font-family:Arial;
                background:#f4f4f4;
                padding:40px'>

        <div style='max-width:600px;
                    margin:auto;
                    background:white;
                    border-radius:12px;
                    padding:40px;
                    text-align:center'>

            <h1 style='color:#dc2626'>
                Reset Your Password
            </h1>

            <p style='font-size:16px;
                      color:#555;
                      line-height:1.7'>
                We received a request to reset your password.
            </p>

            <p style='font-size:16px;
                      color:#555'>
                Click the button below to create a new password.
            </p>

            <a href='{resetPasswordURL}'
               style='display:inline-block;
                      margin-top:20px;
                      background:#dc2626;
                      color:white;
                      padding:15px 30px;
                      text-decoration:none;
                      border-radius:8px;
                      font-weight:bold'>

                Reset Password

            </a>

            <p style='margin-top:30px;
                      color:#888;
                      font-size:14px'>

                If you did not request a password reset,
                you can safely ignore this email.

            </p>

        </div>

    </div>"
                    };

                    // send Email
                    EmailSettings.SendEmail(email, _configuration);
                    return RedirectToAction("CheckYourInbox");

                }

            }
            ModelState.AddModelError("", "invalid Operation");
            return View(nameof(ForgetPassword), forgetPasswordView);
        }


        [HttpGet]
        public IActionResult CheckYourInBox()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();

        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel resetPasswordView)
        {
            if (!ModelState.IsValid) return View(resetPasswordView);
            string email = TempData["email"] as string ?? string.Empty;
            string token = TempData["token"] as string ?? string.Empty;

            //get user by email
            var user = userManager.FindByEmailAsync(email).Result;
            if (user != null)
            {
                var result = userManager.ResetPasswordAsync(user, token, resetPasswordView.Password).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                }
            }
            return View(nameof(ResetPassword), resetPasswordView);

        }
        #endregion


        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = "/")
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/")
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Register");

            // Try to sign in with existing external login
            var result = await signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
                return LocalRedirect(returnUrl);

            // No account yet — create one
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
            var user = new ApplicationUser
            {
                firstName = string.IsNullOrEmpty(firstName) ? email.Split('@')[0] : firstName,

                lastName = string.IsNullOrEmpty(lastName) ? "User" : lastName,

                UserName = email,
                Email = email,
                isAgree = true 
            };

            var createResult = await userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                await userManager.AddLoginAsync(user, info);
                await signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            // Surface errors
            foreach (var error in createResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View("Register");
        }
    }
}
