using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common.Models;
using Web.Data.Abstract;
using WebApplication2.Bussiness.Abstract;

namespace WebApplication2.Bussiness.Derived
{
    public class EmployeesContext : IEmployeesContext
    {

        private readonly IEmployeesService _employeesService;

        public EmployeesContext(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        public EmployeePersonal AccountActivation(EmployeePersonal employee)
        {
            return _employeesService.AccountActivation(employee);
        }

        public EmployeePersonal AccountVerify(EmployeePersonal employee)
        {
            return _employeesService.AccountVerify(employee);
        }

        public EmployeePersonal CreateEmployee(EmployeePersonal employee)
        {
            return _employeesService.CreateEmployee(employee);
        }

        public EmpReg CreateEmpReg(EmpReg emp)
        {
            return _employeesService.CreateEmpReg(emp);
        }

        public EmployeePersonal ForgotPassword(EmployeePersonal employee)
        {
            return _employeesService.ForgotPassword(employee);
        }

        public Task<List<EmployeePersonal>> GetAllEmployeesAsync()
        {
            return _employeesService.GetAllEmployeesAsync();
        }

        


        //public EmployeePersonal GetData(EmployeePersonal employee)
        //{
        //    return _employeesService.GetData(employee);
        //}

        public Task<EmployeePersonal> GetEmployeeById(int id)
        {
            return _employeesService.GetEmployeeById(id);
        }

        public EmployeePersonal LoginData(EmployeePersonal employee)
        {
            return _employeesService.LoginData(employee);
        }

        //public EmployeePersonal Logout(EmployeePersonal employee)
        //{
        //    return _employeesService.Logout(employee);
        //}

        public EmployeePersonal ResetPassword(EmployeePersonal employee)
        {
            return _employeesService.ResetPassword(employee);
        }

      

       
      

      
    }
}
