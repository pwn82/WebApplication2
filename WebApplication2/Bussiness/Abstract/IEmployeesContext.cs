using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common.Models;

namespace WebApplication2.Bussiness.Abstract
{
    public interface IEmployeesContext
    {
        EmployeePersonal ForgotPassword(EmployeePersonal employee);
        EmployeePersonal ResetPassword(EmployeePersonal employee);
        EmployeePersonal AccountVerify(EmployeePersonal employee);
        EmployeePersonal AccountActivation(EmployeePersonal employee);
        EmployeePersonal CreateEmployee(EmployeePersonal employee);
        Task<List<EmployeePersonal>> GetAllEmployeesAsync();
        Task<EmployeePersonal> GetEmployeeById(int id);
        EmployeePersonal LoginData(EmployeePersonal employee);
        EmpReg CreateEmpReg(EmpReg emp);
    }
}
