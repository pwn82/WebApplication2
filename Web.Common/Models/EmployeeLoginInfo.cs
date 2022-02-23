using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.Common.Models
{
    public class EmployeeLoginInfo
    {
        [Key]
        public int Id { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public bool FAEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string LockoutEnd { get; set; }
        public bool LockOutEnabled { get; set; }
    }
}
