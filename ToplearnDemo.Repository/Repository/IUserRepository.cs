using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToplearnDemo.DataLayer.Models.User;
using ToplearnDemo.ViewModels.Account;
using ToplearnDemo.ViewModels.UserPanel;

namespace ToplearnDemo.Repository.Repository
{
    public interface IUserRepository
    {
        #region Register_Login
        Task<bool> UsernameExists(string username);

        Task<bool> EmailExists(string email);

        Task<bool> AddUser(RegisterViewModel user);

        Task<User> LoginUser(LoginViewModel user);

        Task<bool> ActiveAccount(string acrivationCode);

        Task<string> GetActivationCode(string email);

        Task<bool> ResetPassword(ResetPasswordViewModel model);

        Task<bool> ActivatioCodeIsValid(string activationCode);
        #endregion

        #region UserPanel

        Task<UserInformationViewModel> GetUserInformation(string username);

        Task<SideBarUserInformationViewModel> GetUserSideBar(string username);

        Task<EditProfileViewModel> GetEditProfileViewModel(string username);

        Task<bool> EditProfile(string username, EditProfileViewModel model);

        Task<bool> ChangePassword(ChangePasswordViewModel model);

        Task<bool> ComparePassword(string username,string password);
        #endregion
    }
}
