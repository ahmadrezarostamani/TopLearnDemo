using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToplearnDemo.Repository.Repository;
using ToplearnDemo.ViewModels.Account;
using ToplearnDemo.DomainClassess.User;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using ToplearnDemo.Utility.Convertors;
using ToplearnDemo.Utility.Helpers.Interface;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace ToplearnDemo.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IViewRenderService _viewRender;
        private readonly IMessageSender _messageSender;
      
        public AccountController(IUserRepository userRepo,IViewRenderService viewRender,IMessageSender messageSender)
        {
            _userRepo = userRepo;
            _viewRender = viewRender;
            _messageSender = messageSender;
        }

        #region Register

        [Route("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            var viewModel = new ToplearnDemo.Web.ViewModels.Account.RegisterViewModel
            {
                Email = model.Email,
                Name = model.Name,
                Family = model.Family,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                Username = model.Username,
            };
            if (ModelState.IsValid)
            {
                if (await _userRepo.EmailExists(model.Email.FixEmail()))
                {
                    ModelState.AddModelError("Email", "ایمیل وارد شده در دسترس نیست.");
                    return View(viewModel);
                }
                if (await _userRepo.UsernameExists(model.Username.FixUsername()))
                {
                    ModelState.AddModelError("Username", "نام کاربری وارد شده در دسترس نیست.");
                    return View(viewModel);
                }
                var result = await _userRepo.AddUser(model);
                if (result)
                {
                    var message = await _viewRender.RenderToStringAsync("_ActiveAccount", new ActiveAccountViewModel
                    {
                        Username = model.Username,
                        ActivationCode = await _userRepo.GetActivationCode(model.Email)
                    });
                    await _messageSender.SendEmailAsync(model.Email.FixEmail(), "فعالسازی حساب کاربری", message, true);
                    return View("SuccessfulRegister", model);
                }
                else
                {
                    ViewBag.ErrorMessage = "متاسفانه ثبت نام با موفقیت صورت نگرفت.لطفا دوباره تلاش کنید.";
                }
            }

            return View(viewModel);
        }

        #endregion

        #region Login

        [Route("Login")]
        public async Task<IActionResult> Login(string returnUrl=null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            ViewBag.Returnurl = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(ToplearnDemo.ViewModels.Account.LoginViewModel model,string returnUrl=null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            ViewBag.Returnurl = returnUrl;
            if (ModelState.IsValid)
            {
                ToplearnDemo.DomainClassess.User.User user =await  _userRepo.LoginUser(model);
                if(user==null)
                {
                    ModelState.AddModelError("", "نام کاربری یا رمز عبور وارد شده اشتباه است.");
                    return View(model);
                }

                if (!user.IsActive)
                {
                    ModelState.AddModelError("Email"
                        , "حساب کاربری شما فعال نشده است.لطفا با مرجعه به ایمیل خود نسبت به فعالسازی ایمیل خود اقدام نمایید.");
                    return View(model);
                }
                else
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.Username),
                        new Claim(ClaimTypes.Email,user.Email)
                    };

                   
                    var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties()
                    {
                        IsPersistent = model.RememberMe
                    };

                    await HttpContext.SignInAsync(principal,properties);
  
                    ViewBag.SuccessfulLogin = true;
                    HttpContext.Session.SetString("email", model.Email.FixEmail());
                }
                
            }
            
            return View(model);
        }

        #endregion

        #region Logout

        [Route("Logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        #endregion

        #region AccountActivation

        public async Task<IActionResult> ActiveAccount(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ViewBag.ActivationResult = await _userRepo.ActiveAccount(id);
            return View();
        }

        #endregion

        #region ForgotPassword

        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [Route("ForgotPassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {               
                if (await _userRepo.EmailExists(model.Email))
                {
                    var message =await  _viewRender.RenderToStringAsync("_ResetPassword", new SendResetPasswordEmailViewModel
                    {
                        ActivationCode =await  _userRepo.GetActivationCode(model.Email)
                    });

                    await _messageSender.SendEmailAsync(model.Email, "بازنشانی رمز عبور", message, true);
                }
            }
            ViewBag.message = "در صورت معتبر بودن ایمیل وارد شده،لینک بازیابی رمز عبور به آدرس ایمیلتان ارسال خواهد شد.";
            return View();
        }
        #endregion

        #region ResetPassword

        public async Task<IActionResult> ResetPassword(string id)
        {
            ViewBag.ActivationCodeIsValid = false;
            if(!string.IsNullOrEmpty(id))
            {
                ViewBag.ActivationCodeIsValid = await _userRepo.ActivatioCodeIsValid(id);
            }
            if (ViewBag.ActivationCodeIsValid)
            {
                return View(new ResetPasswordViewModel { ActivationCode = id });
            }
            return View();
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            ViewBag.ResetPasswordResult = await _userRepo.ResetPassword(model);
            ViewBag.ActivationCodeIsValid = true;
            return View();
            
        }
        #endregion

        public async Task<IActionResult> EmailExists(string email)
        {
            if (await _userRepo.EmailExists(email))
            {
                return Json("ایمیل وارد شده در دسترس نیست.");
            }
            return Json(true);
        }
        public async Task<IActionResult> UsernameExists(string username)
        {
            if (await _userRepo.UsernameExists(username))
            {
                return Json("نام کاربری وارد شده در دسترس نیست.");
            }
            return Json(true);
        }
    }
}
