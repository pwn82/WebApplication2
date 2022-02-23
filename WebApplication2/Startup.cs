using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common.DB;
using Web.Data.Abstract;
using Web.Data.Derived;
using Web.Security;
using WebApplication2.Bussiness.Abstract;
using WebApplication2.Bussiness.Derived;

namespace WebApplication2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebContext>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("DbConnectionString")));
            services.AddCors(opt => opt.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            }));
            services.AddTransient<IEmployeesContext, EmployeesContext>();
            services.AddTransient<IEmployeesService, EmployeesService>();
            services.AddTransient<IEmpContext, EmpContext>();
            services.AddTransient<IEmpService, EmpService>();
            services.AddTransient<ITokenValidation, TokenValidation>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMiddleware<AuthenticationMiddleware>();
            app.UseHttpsRedirection();
            app.UseCors();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
