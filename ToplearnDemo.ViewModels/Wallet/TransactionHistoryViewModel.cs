using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace ToplearnDemo.ViewModels.Wallet
{
    public class TransactionHistoryViewModel
    {
        [Display(Name = "نوع تراکنش")]
        public int TransactionType { get; set; }

        [Display(Name = "تاریخ تراکنش")]
        public DateTime PaidDate { get; set; }

        [Display(Name = "شرح")]
        public string Description { get; set; }

        [Display(Name = "مبلغ")]
        public int Price { get; set; }

    }
}
