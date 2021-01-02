using System;
using System.Collections.Generic;
using System.Text;

namespace ToplearnDemo.DataLayer.Models.User
{
    public class UserRole
    {
        public UserRole()
        {

        }
        public int UserRoleId { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
