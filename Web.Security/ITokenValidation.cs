using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Security
{
    public interface ITokenValidation
    {
        bool ValidateToken(string token);

        string GenerateToken();

        bool SendEmailForResetPassword(string toEmailId, string url);

        bool SendEmailForAccountVerify(string toEmailId, string url);
    }
}
