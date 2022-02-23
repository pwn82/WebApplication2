using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.Common.Models
{
    public class EmployeePersonal
    {
        [Key]
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
        public string RelationName { get; set; }
        public long? Phone { get; set; }
        public long? EmergencyNumber { get; set; }
        public string Email { get; set; }
        public string PermanentAddress { get; set; }
        public string CurrentAddress { get; set; }
        public string Token { get; set; }
        
        public DateTime? TokenExpiry { get; set; }
        public int? InvalidCount { get; set; }
        public bool? Status { get; set; }
    }
}
