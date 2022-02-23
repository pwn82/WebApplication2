using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.Common;
using Web.Common.DB;
using Web.Common.Models;
using WebApplication2.Bussiness.Abstract;
using static Web.Common.CustomException;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeesContext _employeeContext;
        private readonly WebContext _webContext;
        IConfiguration _configuration;
        public EmployeeController(IEmployeesContext employeeContext, WebContext webContext, IConfiguration configuration)
        {
            _employeeContext = employeeContext;
            _webContext = webContext;
            _configuration=configuration;
         }
        [HttpPost("Login")]
        public IActionResult Login(EmployeePersonal employee)
        {
            EmployeePersonal result = _employeeContext.LoginData(employee);
            EmployeePersonal details = _webContext.employeePersonals.Where(x => x.Username == employee.Username).FirstOrDefault();
            if (result != null && details.Status == true)
            {
                return Ok(new { status = HttpStatusCode.OK, success = true, data = result });
            }
            else if (result != null && details.Status == false)
            {
                JsonError k = new CustomException(ErrorCodes.Valid_block.ToString(), _configuration).GetErrorObject();
                return Ok(k);
            }
            else if (result == null && details == null)
            {
                JsonError k = new CustomException(ErrorCodes.Not_Found.ToString(), _configuration).GetErrorObject();
                return Ok(k);
            }
            else if (result == null && details.InvalidCount < 3)
            {
                JsonError k = new CustomException(ErrorCodes.Invalid.ToString(), _configuration).GetErrorObject();
                return Ok(k);
            }
            else if (result == null && details.InvalidCount >= 3)
            {
                JsonError k = new CustomException(ErrorCodes.Blocked.ToString(), _configuration).GetErrorObject();
                return Ok(k);
            }
            else
            {
                JsonError k = new CustomException(ErrorCodes.Unauthorized.ToString(), _configuration).GetErrorObject();
                return Ok(k);
            }
        }

        /// <summary>
        /// This request is to create the new user with required details.
        /// </summary>
        /// <param name="Employee"></param>
        /// <returns></returns>
        [HttpPost("Registration")]
        public ActionResult CreateEmployee(EmployeePersonal Employee)
        {
            EmployeePersonal employee = _employeeContext.CreateEmployee(Employee);
            if (employee != null)
            {
                return Ok(new { status = HttpStatusCode.OK, success = true, data = "Registration successful" });
            }
            else
            {
                return Ok(new { status = HttpStatusCode.BadRequest, success = false, data = "User not registered" });
            }
        }

        [HttpPost("Regis")]
        public ActionResult CreateEmpReg(EmpReg Employee)
        {
            EmpReg employee = _employeeContext.CreateEmpReg(Employee);
            if (employee != null)
            {
                return Ok(new { status = HttpStatusCode.OK, success = true, data = "Registration successful" });
            }
            else
            {
                return Ok(new { status = HttpStatusCode.BadRequest, success = false, data = "User not registered" });
            }
        }

    }
}
