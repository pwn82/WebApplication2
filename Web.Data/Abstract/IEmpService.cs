using System;
using System.Collections.Generic;
using System.Text;
using Web.Common.Models;

namespace Web.Data.Abstract
{
    public interface IEmpService
    {
        EmpReg CreateEmployee(EmpReg emp);
    }
}
