using System;
using System.Collections.Generic;
using System.Text;

namespace ToplearnDemo.ViewModels.Wallet
{
    public class PaymentResultViewModel
    {
        public string PaymentCode { get; set; }

        public bool Status { get; set; }

        public string ErrorMessage { get; set; }

    }
}
