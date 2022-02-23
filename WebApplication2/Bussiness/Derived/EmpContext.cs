using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common.Models;
using Web.Data.Abstract;
using WebApplication2.Bussiness.Abstract;

namespace WebApplication2.Bussiness.Derived
{
    public class EmpContext : IEmpContext
    {
        private readonly IEmpService _empService;
        public EmpContext(IEmpService empService)
        {
            _empService = empService;
        }
        public EmpReg CreateEmployee(EmpReg emp)
        {
            return _empService.CreateEmployee(emp);
        }
    }
}
