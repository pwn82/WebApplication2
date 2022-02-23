using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common.Models;

namespace WebApplication2.Bussiness.Abstract
{
    public interface IEmpContext
    {
        EmpReg CreateEmployee(EmpReg emp);
    }
}
