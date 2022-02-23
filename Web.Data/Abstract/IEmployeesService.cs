using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Common.Models;

namespace Web.Data.Abstract
{
    public interface IEmployeesService
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
