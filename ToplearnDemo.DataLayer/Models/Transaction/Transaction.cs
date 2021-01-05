using System;
using System.Collections.Generic;
using System.Text;
using ToplearnDemo.DataLayer.Models.User;
using System.ComponentModel.DataAnnotations;
namespace ToplearnDemo.DataLayer.Models.Transaction
{
    public class Transaction
    {
        public Transaction()
        {

        }

        public int TransactionId { get; set; }

        [Display(Name ="کاربر")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
        public int UserId { get; set; }

        [Display(Name = "نوع تراکنش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int TransactionTypeId { get; set; }


        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int Price { get; set; }

        [Display(Name = "تایید شده")]
        public bool IsDone { get; set; }

        [Display(Name = "شرح")]
        [MaxLength(500,ErrorMessage ="{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        public string Description { get; set; }

        [Display(Name = "تاریخ تراکنش")]
        public DateTime PaidDate { get; set; }


        public virtual User.User User { get; set; }

        public virtual TransactionType TransactionType { get; set; }

    }
}
