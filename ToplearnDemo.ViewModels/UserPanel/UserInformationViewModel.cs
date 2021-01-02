using System;
using System.Collections.Generic;
using System.Text;

namespace ToplearnDemo.ViewModels.UserPanel
{
    public class UserInformationViewModel
    {
        public string Name { get; set; }
        public string Family { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredDate { get; set; }
        public decimal Wallet { get; set; }
    }
}
