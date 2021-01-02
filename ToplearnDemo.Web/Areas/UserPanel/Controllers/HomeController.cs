using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToplearnDemo.Repository.Repository;
using ToplearnDemo.Utility.Convertors;
using ToplearnDemo.ViewModels.UserPanel;
using Microsoft.AspNetCore.Http;

namespace ToplearnDemo.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepo;
        public HomeController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _userRepo.GetUserInformation(User.Identity.Name));
        }

        #region EditProfile
        [Route("UserPanel/[action]")]
        public async Task<IActionResult> EditProfile()
        {
            var x = await _userRepo.GetEditProfileViewModel(User.Identity.Name);

            return View(new ViewModels.UserPanel.EditProfileViewModel
            {
                Name = x.Name,
                Family = x.Family,
                Email = x.Email,
                UserImage = x.UserImage,
            });
        }


        [Route("UserPanel/[action]")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            var viewModel = new ViewModels.UserPanel.EditProfileViewModel
            {
                Name = model.Name,
                Family = model.Family,
                Email = model.Email,
                UserImage = model.UserImage,
            };

            string currentEmail = HttpContext.Session.GetString("email");

            if (ModelState.IsValid)
            {
                if (model.Email.FixEmail() != currentEmail)
                {
                    if (await _userRepo.EmailExists(model.Email.FixEmail()))
                    {
                        ModelState.AddModelError("Email", "ایمیل وارد شده در دسترس نیست.");
                        return View(viewModel);
                    }
                }


                ViewBag.EditProfileResult = await _userRepo.EditProfile(User.Identity.Name, model);
            }

            return View(viewModel);
        }
        #endregion


        #region ChangePassword
        [Route("[area]/[action]")]
        public async Task<IActionResult> ChangePassword()
        {
            return View(new ViewModels.UserPanel.ChangePasswordViewModel { Username=User.Identity.Name});
        }

        [Route("[area]/[action]")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var viewModel = new ViewModels.UserPanel.ChangePasswordViewModel
            {
                Username = User.Identity.Name,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                OldPassword = model.OldPassword
            };
            if(ModelState.IsValid)
            {
                if(!await _userRepo.ComparePassword(User.Identity.Name,model.OldPassword))
                {
                    ModelState.AddModelError("OldPassowrd", "کلمه عبور وارد شده صحیح نیست.");
                    return View(viewModel);
                }

                ViewBag.ChangePasswordResult =await  _userRepo.ChangePassword(model);
            }
            return View(viewModel);
        }
        #endregion


        public async Task<IActionResult> EmailExists(string email)
        {
            email = email.FixEmail();
            var currentEmail = HttpContext.Session.GetString("email");
            if (email==currentEmail)
            {
                return Json(true);
            }
           
            if (await _userRepo.EmailExists(email))
            {
                return Json("ایمیل وارد شده در دسترس نیست.");
            }
            return Json(true);

        }

        public async Task<IActionResult> ComparePassword(string OldPassword)
        {
            if (await _userRepo.ComparePassword(User.Identity.Name,OldPassword))
            {
                return Json(true);
            }
            return Json("کلمه عبور وارد شده صحیح نیست.");
        }
    }
}
