using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace ToplearnDemo.DataLayer.Models.Transaction
{
    public class TransactionType
    {
        public TransactionType()
        {

        }
        public int TransactionTypeId { get; set; }

        [Display(Name ="عنوان تراکنش")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
        [MaxLength(30,ErrorMessage ="{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]

        public string TransactionTitle { get; set; }

        public virtual IEnumerable<Transaction> Transactions { get; set; }
    }
}
