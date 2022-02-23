using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Common
{
    public class JsonError
    {
        public string Status { get; set; }
        public CustomError Error { get; set; }
    }

    public class CustomError
    {
        public string Detail { get; set; }
    }

    public class ApiUrl
    {
        public static string Login = "api/Employee/Login";
        public static string ForgotPassword = "api/Employee/ForgotPassword";
        public static string ResetPassword = "api/Employee/ResetPassword";
        public static string AccountVerify = "api/Employee/AccountVerify";
        public static string AccountActivation = "api/Employee/AccountActivation";
        public static string EmployeeRegistration = "api/Employee/Registration";
        public static string PatientRegistration = "api/Patient/Registration";
        public static string DoctorRegistration = "api/Doctor/Registration";

    }
}
