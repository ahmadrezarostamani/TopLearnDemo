using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ToplearnDemo.DomainClassess.User
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Display(Name ="عنوان نقش")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
        [MaxLength(200,ErrorMessage ="{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        public string RoleTitle { get; set; }


        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
