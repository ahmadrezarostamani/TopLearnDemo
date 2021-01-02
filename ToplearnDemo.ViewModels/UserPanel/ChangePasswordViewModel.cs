using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ToplearnDemo.ViewModels.UserPanel
{
    public class ChangePasswordViewModel
    {
        public string Username { get; set; }

        [Display(Name ="رمز عبور فعلی")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
        [MaxLength(50,ErrorMessage ="{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }


        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "تکرار رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        [Compare("Password",ErrorMessage ="رمز عبور و تکرار آن مطابقت ندارند.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
