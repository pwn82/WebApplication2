using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Common
{
    public class CustomException : Exception
    {
        string _errorCode;
        IConfiguration _configuration;

        public CustomException(string errorCode, IConfiguration configuration)
        {
            _errorCode = errorCode;
            _configuration = configuration;
        }

        public JsonError GetErrorObject()
        {
            return new JsonError()
            {
                Status = _configuration[_errorCode + ":" + "status"],
                Error = new CustomError
                {
                    Detail = _configuration[_errorCode + ":" + "detail"]
                }
            };
        }

        public enum ErrorCodes
        {
            Token_Header_Missing,
            Payload_Missing,
            Signature_Missing,
            Token_Expired,
            Not_ProperToken,
            Valid_block,
            Not_Found,
            Invalid,
            Blocked,
            Unauthorized,
            Success,
            Default
        }

    }
}
