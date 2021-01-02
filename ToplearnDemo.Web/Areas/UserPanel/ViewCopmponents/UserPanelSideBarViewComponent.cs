using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToplearnDemo.Repository.Repository;

namespace ToplearnDemo.Web.Areas.UserPanel.ViewCopmponents
{
    public class UserPanelSideBarViewComponent:ViewComponent
    {
        private readonly IUserRepository _userRepo;
        public UserPanelSideBarViewComponent(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("UserPanelSideBar", await _userRepo.GetUserSideBar(User.Identity.Name)));
        }
    }
}
