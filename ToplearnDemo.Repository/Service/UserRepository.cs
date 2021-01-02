using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToplearnDemo.DataLayer.Context;
using ToplearnDemo.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using ToplearnDemo.ViewModels.Account;
using ToplearnDemo.DataLayer.Models.User;
using AutoMapper;
using ToplearnDemo.Utility.Generators;
using ToplearnDemo.Utility.Helpers;
using ToplearnDemo.Utility.Convertors;
using ToplearnDemo.ViewModels.UserPanel;
using System.IO;

namespace ToplearnDemo.Repository.Service
{
    public class UserRepository : IUserRepository
    {
        private readonly TopleranDemoContext _db;
        private readonly IMapper _mapper;
        public UserRepository(TopleranDemoContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> ResetPassword(ResetPasswordViewModel model)
        {
            var user =await _db.Users.FirstOrDefaultAsync(user => user.ActivationCode == model.ActivationCode);
            if(user!=null)
            {
                user.Password = HashHelper.ComputeHash(model.Password);
                user.ActivationCode = NameGenerator.GenerateUniqueString();
            }

            return await Save();
        }


        public async Task<string> GetActivationCode(string email)
        {
            var user = await _db.Users.FirstAsync(u => u.Email == email.FixEmail());
            return user.ActivationCode;
        }

        public async Task<bool> ActiveAccount(string acrivationCode)
        {
            var user =await _db.Users.FirstOrDefaultAsync(u => u.ActivationCode == acrivationCode);
            if(user==null)
            {
                return false;
            }

            user.IsActive = true;
            user.ActivationCode = NameGenerator.GenerateUniqueString();
            return await Save();
        }

        public async Task<bool> AddUser(RegisterViewModel model)
        {
            User user = _mapper.Map<User>(model);
            user.ActivationCode = NameGenerator.GenerateUniqueString();
            user.RegisteredDate = DateTime.Now;
            user.Email = user.Email.FixEmail();
            user.Username = user.Username.FixUsername();
            user.Name = user.Name.FixUsername();
            user.Family = user.Family.FixUsername();
            user.IsActive = false;
            user.Password = HashHelper.ComputeHash(model.Password);
            user.UserImage = "Default.jpg";
            await _db.Users.AddAsync(user);
            return await Save();           
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> LoginUser(LoginViewModel user)
        {
            var hashedPassword = HashHelper.ComputeHash(user.Password);
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == user.Email.FixEmail() && u.Password==hashedPassword);
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await _db.Users.AnyAsync(u => u.Username == username);
        }

        private async Task<bool> Save()
        {
            if (await _db.SaveChangesAsync()>0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ActivatioCodeIsValid(string activationCode)
        {
            return await _db.Users.AnyAsync(user => user.ActivationCode == activationCode);
        }

        #region UserPanel

        public async Task<UserInformationViewModel> GetUserInformation(string username)
        {
            var user =await _db.Users.FirstOrDefaultAsync(User => User.Username == username);
            return new UserInformationViewModel
            {
                Name=user.Name,
                Family=user.Family,
                UserName = user.Username,
                Email = user.Email,
                Wallet = 0,
                RegisteredDate = user.RegisteredDate
            };
        }

        public async Task<SideBarUserInformationViewModel> GetUserSideBar(string username)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Username == username);
            return new SideBarUserInformationViewModel
            {
                Username = user.Username,
                UserImage = user.UserImage,
                RegisteredDate = user.RegisteredDate
            };
        }

        public async Task<EditProfileViewModel> GetEditProfileViewModel(string username)
        {
            var user = await _db.Users.FirstAsync(user => user.Username == username);
            return new EditProfileViewModel
            {
                Name=user.Name,
                Family=user.Family,
                Email = user.Email,
                UserImage = user.UserImage
            };
        }

        public async Task<bool> EditProfile(string username, EditProfileViewModel model)
        {
            User user = await _db.Users.FirstAsync(user => user.Username == username);
            //if(await EmailExists(model.Email.FixEmail()))
            //{
            //    return false;
            //}
            var imagepath = "";
            if (model.UserAvatar!=null)
            {
                if (user.UserImage!= "Default.jpg")
                {
                    imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserImages", user.UserImage);
                    if(File.Exists(imagepath))
                    {
                        File.Delete(imagepath);
                    }
                }

                model.UserImage = NameGenerator.GenerateUniqueString() + Path.GetExtension(model.UserAvatar.FileName);
                imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserImages", model.UserImage);
                try
                {
                    using (var stream = new FileStream(imagepath, FileMode.Create))
                    {
                        await model.UserAvatar.CopyToAsync(stream);
                    }
                }
                catch(Exception ex)
                {
                    
                }

            }

            user.Email = model.Email.FixEmail();
            user.Name = model.Name.FixUsername();
            user.Family = model.Family.FixUsername();
            user.UserImage = model.UserImage;
            return await Save();
            
        }

        public async Task<bool> ChangePassword(ChangePasswordViewModel model)
        {
            var user =await  _db.Users.FirstAsync(user => user.Username == model.Username);
            if(user.Password==HashHelper.ComputeHash(model.OldPassword))
            {
                user.Password = HashHelper.ComputeHash(model.Password);
                return await Save();
            }

            return false;
        }

        public async Task<bool> ComparePassword(string username, string password)
        {
            var user = await _db.Users.FirstAsync(user =>user.Username==username);
            return user.Password == HashHelper.ComputeHash(password);
        }
        #endregion

    }
}
