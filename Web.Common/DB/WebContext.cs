using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common.Models;

namespace Web.Common.DB
{
    public class WebContext:DbContext
    {
        public WebContext(DbContextOptions<WebContext>options):base(options)
        {

        }
        public DbSet<EmployeePersonal> employeePersonals { get; set; }
        public DbSet<EmployeeLoginInfo> employeeLoginInfos { get; set; }
        public DbSet<EmpReg> empRegs { get; set; }
    }
}
