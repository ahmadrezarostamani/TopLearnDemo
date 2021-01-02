using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace ToplearnDemo.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Display(Name ="ایمیل")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
        [EmailAddress(ErrorMessage ="{0} وارد شده معتبر نیست.")]
        public string Email { get; set; }
    }
}
