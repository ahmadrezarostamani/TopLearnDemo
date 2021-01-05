using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace ToplearnDemo.ViewModels.Wallet
{
    public class ChargeWalletViewModel
    {
        [Display(Name ="مبلغ")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
        [DataType(DataType.Currency)]
        public int Price { get; set; }
    }
}
