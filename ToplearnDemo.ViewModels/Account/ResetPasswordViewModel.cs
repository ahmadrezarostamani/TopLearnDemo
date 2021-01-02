using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace ToplearnDemo.ViewModels.Account
{
    public class ResetPasswordViewModel
    {
        [Display(Name ="کلمه عبور جدید")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
        [MaxLength(50,ErrorMessage ="{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "کلمه عبور و تکرار آن مطابقت ندارند.")]
        public string ConfirmPassword { get; set; }

        public string ActivationCode { get; set; }

    }
}
