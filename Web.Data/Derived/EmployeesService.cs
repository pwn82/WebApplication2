using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Web.Common.Models;
using Web.Data.Abstract;
using Web.Security;
using Web.Common.DB;
using System.Net.Mail;

namespace Web.Data.Derived
{
   public class EmployeesService: IEmployeesService
    {
        private readonly ITokenValidation _tokenValidation;
        private readonly WebContext _webContext;
        public EmployeesService(ITokenValidation tokenValidation,WebContext webContext)
        {
            _tokenValidation = tokenValidation;
            _webContext = webContext;
        }
        public async Task<List<EmployeePersonal>> GetAllEmployeesAsync()
        {
            try
            {
                return await _webContext.employeePersonals.ToListAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<EmployeePersonal>GetEmployeeById(int id)
        {
            EmployeePersonal employeePersonal;
            try
            {
                employeePersonal = _webContext.Find<EmployeePersonal>(id);
            }
            catch(Exception)
            {
                throw;
            }
            return employeePersonal;
        }
        public EmployeePersonal CreateEmployee(EmployeePersonal employee)
        {
            string date;

            var value = GetEmployeeById(employee.EmployeeId);
            if (value != null)
            {
               // employee.EmployeeId = 1;
                date = String.Format("{0:dd/MM/yyyy}", employee.Dob);
                employee.Dob = date;
                employee.InvalidCount = 0;
                employee.Status = true;
                employee.Password = EncryptPassword(employee.Password);
                employee.ConfirmPassword = EncryptPassword(employee.ConfirmPassword);
                _webContext.Update(employee);
                _webContext.SaveChanges();
                return employee;
            }
            else
            {               
                date = String.Format("{0:dd/MM/yyyy}", employee.Dob);
                employee.Dob = date;
                employee.InvalidCount = 0;
                employee.Status = true;
                employee.Password = EncryptPassword(employee.Password);
                employee.ConfirmPassword = EncryptPassword(employee.ConfirmPassword);
                _webContext.Add(employee);
                _webContext.SaveChanges();
                return employee;
            }
           

        }

        public string EncryptPassword(string password)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    password = Convert.ToBase64String(ms.ToArray());
                }
            }
            return password;
        }

        public string DecryptPassword(string password)
        {
            string EncryptionKeys = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKeys, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    password = Encoding.Unicode.GetString(ms.ToArray());
                    return password;
                }
            }
        }
        public EmployeePersonal ForgotPassword(EmployeePersonal employee)
        {
            EmployeePersonal model = _webContext.employeePersonals.Where(x => x.EmployeeId == employee.EmployeeId).FirstOrDefault();
            if (model != null)
            {
                string token = _tokenValidation.GenerateToken();
                string link = "http://localhost:4200/index/resetpassword?Token=" + token + "&Username=" + model.Username;
                _tokenValidation.SendEmailForResetPassword(model.Email, link);
                DateTime currentTime = DateTime.Now;
                TimeSpan addMinutes = new TimeSpan(0, 0, 5, 0);
                DateTime expiryTime = currentTime.Add(addMinutes);
                model.TokenExpiry = expiryTime;
                model.Token = token;
                return model;
            }
            else
            {
                return model;
            }
        }
        public EmployeePersonal ResetPassword(EmployeePersonal employee)
        {
            EmployeePersonal model = _webContext.employeePersonals.Where(x => x.Username == employee.Username && x.Token == employee.Token).FirstOrDefault();
            if (model != null)
            {
                if (DateTime.Now <= model.TokenExpiry)
                {
                    model.Password = EncryptPassword(employee.Password);
                    model.ConfirmPassword = EncryptPassword(employee.Password);
                    //_webContext.employeePersonals.ReplaceOne(x =>
                    //x.EmployeeId == model.EmployeeId, model);
                    return model;
                }
                else
                {
                    return model;
                }
            }
            return model;
        }
        public EmployeePersonal AccountVerify(EmployeePersonal employee)
        {
            EmployeePersonal model = _webContext.employeePersonals.Where(x => 
            x.Username == employee.Username).FirstOrDefault();
            if (model != null)
            {
                EmployeePersonal status = _webContext.employeePersonals.Where(x => 
                x.Username == employee.Username && x.Status == false).FirstOrDefault();
                if (status != null)
                {
                    string token = _tokenValidation.GenerateToken();
                    string link = "http://localhost:4200/index/activateaccount?Token=" + token + "&Username=" + model.Username;
                    _tokenValidation.SendEmailForAccountVerify(model.Email, link);
                    model.Token = token;
                   // _webContext.ReplaceOne(x => x.PersonalDetails.EmployeeId == model.PersonalDetails.EmployeeId, model);
                    return model;
                }
                else
                {
                    return model;
                }
            }
            else
            {
                return model;
            }
        }

        public EmployeePersonal AccountActivation(EmployeePersonal employee)
        {
            EmployeePersonal model = _webContext.employeePersonals.Where(x => 
            x.Username == employee.Username
            && x.Token == employee.Token && x.Dob == employee.Dob).FirstOrDefault();
            if (model != null)
            {
                model.InvalidCount = 0;
                model.Status = true;
                model.TokenExpiry = null;
                model.Token = null;
               // _webContext.employeePersonals.replace(x => x.EmployeeId == model.EmployeeId, model);
                return model;
            }
            else
            {
                return model;
            }
        }
        public EmployeePersonal LoginData(EmployeePersonal employee)
        {
            try
            {
                EmployeePersonal model = _webContext.employeePersonals.Where(x => x.Username == employee.Username).FirstOrDefault();
                if (model != null)
                {
                    var password = DecryptPassword(model.Password);
                    EmployeePersonal data = _webContext.employeePersonals.Where(x => x.Username == employee.Username && password == employee.Password).FirstOrDefault();
                    if (data != null)
                    {
                        var accountStatus = _webContext.employeePersonals.Where(x => x.Username == employee.Username && x.InvalidCount >= 3).FirstOrDefault();
                        if (accountStatus == null)
                        {
                            string token = _tokenValidation.GenerateToken();
                            DateTime d = DateTime.Now;
                            TimeSpan t = new TimeSpan(0, 5, 0);
                            model.Token = token;
                            model.TokenExpiry = d.Add(t);
                            model.InvalidCount = 0;
                            model.Status = true;
                            _webContext.Update(model);
                            _webContext.SaveChanges();
                            return model;
                        }
                        else
                        {
                            model.Status = false;
                            _webContext.Update(model);
                            
                            return model;
                        }
                    }
                    else
                    {
                        model.InvalidCount = model.InvalidCount + 1;
                        _webContext.Update(model);

                        var accountStatus = _webContext.employeePersonals.Where(x => x.Username == employee.Username && x.InvalidCount >= 3).FirstOrDefault();
                        if (accountStatus == null)
                        {
                            _webContext.SaveChanges();
                            return data;
                        }
                        else
                        {
                            model.Status = false;
                           _webContext.Update(model);
                            _webContext.SaveChanges();
                            return data;
                        }
                         
                    }
                }
                else
                {
                    return model;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmpReg CreateEmpReg(EmpReg emp)
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
