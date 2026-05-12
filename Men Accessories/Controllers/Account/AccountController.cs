using Men_Accessories.Models;
using Men_Accessories.Utilities;
using Men_Accessories.ViewModels.Account;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Men_Accessories.Controllers.Account
{
    public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : Controller
    {

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel registerview)
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
                        Body = $"Please confirm your account by clicking this link: {confirmEmailURL}"
                    };

                    EmailSettings.SendEmail(email);

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
        public IActionResult ConfirmEmail(string email , string token)
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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginView)
        {
            if (!ModelState.IsValid) return View(loginView);
            var user = userManager.FindByEmailAsync(loginView.Email).Result;
            if (user is not null)
            {
                bool flagCorrect = userManager.CheckPasswordAsync(user, loginView.Password).Result;
                if (flagCorrect)
                {
                    if (!user.EmailConfirmed)
                    {
                        ModelState.AddModelError("", "Please confirm your email before logging in");
                        return View(loginView);
                    }
                    var result = signInManager.PasswordSignInAsync(user, loginView.Password, loginView.RememberMe, false).Result;
                    if (result.IsNotAllowed)
                        ModelState.AddModelError("", "Your Account Is Not Allowed to login");
                    if (result.IsLockedOut)
                        ModelState.AddModelError("", "Your Account Is Locked Out");
                    if (result.Succeeded)
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Login");
            }
            return View(loginView);

        }

        #endregion

        public async Task<IActionResult> SignOut()
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
                if(user is not null)
                {
                    var token = userManager.GeneratePasswordResetTokenAsync(user).Result;

                    var resetPasswordURL = Url.Action("ResetPassword", "Account", new { email = forgetPasswordView.Email, token },Request.Scheme);
                    //create Email
                    var email = new Email()
                    {
                        To = forgetPasswordView.Email,
                        Subject = "Reset Password",
                        Body = resetPasswordURL 
                    };

                    // send Email
                    EmailSettings.SendEmail(email);
                    return RedirectToAction("CheckYourInbox");

                }

            }
            ModelState.AddModelError("", "invalid Operation");
            return View(nameof(ForgetPassword),forgetPasswordView);
        }


        [HttpGet]
        public IActionResult CheckYourInBox()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword(string email,string token)
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
            if(user !=null)
            {
                var result = userManager.ResetPasswordAsync(user, token, resetPasswordView.Password).Result;
                if(result.Succeeded)
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

    }
}
