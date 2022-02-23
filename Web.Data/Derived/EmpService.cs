using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Web.Common.DB;
using Web.Common.Models;
using Web.Data.Abstract;
using Web.Security;

namespace Web.Data.Derived
{
    public class EmpService : IEmpService
    {
        private readonly ITokenValidation _tokenValidation;
        private readonly WebContext _webContext;
        public EmpService(WebContext webContext,ITokenValidation tokenValidation)
        {
            _tokenValidation = tokenValidation;
            _webContext = webContext;
        }
        public EmpReg CreateEmployee(EmpReg emp)
        {
            string passWord = GeneratePassword().ToString();
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("pypkumar@gmail.com");
            msg.To.Add(emp.email);
            msg.Subject = "New Password for your Account";
            msg.Body = "Your New Password is:" + passWord;
            msg.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            System.Net.NetworkCredential network = new System.Net.NetworkCredential();
            network.UserName = "pypkumar@gmail.com";
            network.Password = "pypkumar82";
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = network;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(msg);
            _webContext.Update(emp);
            _webContext.SaveChanges();
            return emp;
        }
        public string GeneratePassword()
        {
            string PasswordLength = "8";
            string NewPassword = "";

            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";


            char[] sep = {
            ','
        };
            string[] arr = allowedChars.Split(sep);


            string IDString = "";
            string temp = "";

            Random rand = new Random();

            for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                NewPassword = IDString;

            }
            return NewPassword;
        }
    }
}
