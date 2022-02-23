using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.Common.DB;
using Web.Common.Models;
using WebApplication2.Bussiness.Abstract;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpRegController : ControllerBase
    {
        private readonly IEmpContext _emp;             
        public EmpRegController(IEmpContext emp, IConfiguration configuration)
        {          
            _emp = emp;          
        }
        [HttpPost("Reg")]
        public ActionResult CreateEmployee(EmpReg Employee)
        {
            EmpReg employee = _emp.CreateEmployee(Employee);
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
