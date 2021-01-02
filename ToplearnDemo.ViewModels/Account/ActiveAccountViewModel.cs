using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace ToplearnDemo.ViewModels.Account
{
    public class ActiveAccountViewModel
    {

        [Display(Name ="نام کاربری")]
        public string Username { get; set; }

        [Display(Name = "کد فعالسازی")]
        public string ActivationCode { get; set; }
    }
}
