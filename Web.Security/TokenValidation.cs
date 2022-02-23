using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Web.Common;
using static Web.Common.CustomException;

namespace Web.Security
{
    public class TokenValidation : ITokenValidation
    {
        IConfiguration _configuration;

        public TokenValidation(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken()
        {
            var signinKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));
            int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

            var tokenData = new JwtSecurityToken(
              issuer: _configuration["Jwt:Issuer"],
              audience: _configuration["Jwt:Audience"],
              expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
              signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
            );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenData);
            return token;
        }

        public bool ValidateToken(string token)
        {
            if (token == null)
            {
                throw new CustomException(ErrorCodes.Token_Header_Missing.ToString(), _configuration);
            }

            var handler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = null;
            ClaimsPrincipal principal = null;

            try
            {
                var tokenSecure = handler.ReadToken(token);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("payload"))
                {
                    throw new CustomException(ErrorCodes.Payload_Missing.ToString(), _configuration);
                }
                else if (ex.Message.Contains("header"))
                {
                    throw new CustomException(ErrorCodes.Token_Header_Missing.ToString(), _configuration);
                }
                else
                {
                    throw new CustomException(ErrorCodes.Not_ProperToken.ToString(), _configuration);
                }
            }


            var validations = new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]))
            };

            try
            {
                principal = handler.ValidateToken(token, validations, out securityToken);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Signature validation failed") || ex.Message.Contains("signature"))
                {
                    throw new CustomException(ErrorCodes.Signature_Missing.ToString(), _configuration);
                }
                else
                {
                    throw new CustomException(ErrorCodes.Not_ProperToken.ToString(), _configuration);
                }
            }

            if (principal != null)
            {
                string tokenTime = principal.Claims.Where(x => x.Type == "exp").Select(x => x.Value).FirstOrDefault();
                string audience = principal.Claims.Where(x => x.Type == "aud").Select(x => x.Value).FirstOrDefault();
                string jwtId = principal.Claims.Where(x => x.Type == "iss").Select(x => x.Value).FirstOrDefault();

                if (tokenTime != null)
                {
                    DateTime expiryTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToInt32(tokenTime));
                    DateTime currentTime = DateTime.Now;
                    if (currentTime > expiryTime)
                    {
                        throw new CustomException(ErrorCodes.Token_Expired.ToString(), _configuration);
                    }
                }
                else if (audience == null || jwtId == null)
                {
                    throw new CustomException(ErrorCodes.Token_Header_Missing.ToString(), _configuration);
                }
                return true;
            }
            else
            {
                throw new CustomException(ErrorCodes.Token_Header_Missing.ToString(), _configuration);
            }
        }

        public bool SendEmailForResetPassword(string toEmailId, string url)
        {
            var fromEmail = new MailAddress("pypkumar@gmail.com", "citiustech healthcare technology");
            var toEmail = new MailAddress(toEmailId);
            var fromEmailPassword = "pypkumar82";
            string subject = "Password recovery URL";
            string body = "Hi,<br/><br/>We got request for reset your account password. Please find the token & click on the below link to reset your password" +
            "<br/><br/><a href=" + url + ">Reset your Password</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            })
                smtp.Send(message);
            return true;
        }

        public bool SendEmailForAccountVerify(string toEmailId, string url)
        {

            var fromEmail = new MailAddress("pypkumar@gmail.com", "citiustech healthcare technology");
            var toEmail = new MailAddress(toEmailId);
            var fromEmailPassword = "pypkumar82";
            string subject = "Account acctivation";
            string body = "Hi,<br/><br/> Kindly click on below link to activate your Account" +
            "<br/><br/><a href=" + url + ">Activate your account</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            })
                smtp.Send(message);
            return true;

        }
    }
}
