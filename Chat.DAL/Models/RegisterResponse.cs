using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Models
{
    public class RegisterResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ErrorCodeType ErrorCode { get; set; }

        public enum ErrorCodeType
        {
            None = 0,
            InvalidUserData = 1,
            EmailExists = 2,
            EmailInvalid = 3,
            PasswordInvalid = 4,
            PasswordMismatch = 5
        }

        public bool Success
        {
            get { return ErrorCode == ErrorCodeType.None; }
        }

        public static RegisterResponse EmailExists
        {
            get
            {
                var res = new RegisterResponse();
                res.ErrorCode = ErrorCodeType.EmailExists;
                return res;
            }
        }

        public static RegisterResponse EmailInvalid
        {
            get
            {
                var res = new RegisterResponse();
                res.ErrorCode = ErrorCodeType.EmailInvalid;
                return res;
            }
        }

        public static RegisterResponse PasswordInvalid
        {
            get
            {
                var res = new RegisterResponse();
                res.ErrorCode = ErrorCodeType.PasswordInvalid;
                return res;
            }
        }

        public static RegisterResponse PasswordMismatch
        {
            get
            {
                var res = new RegisterResponse();
                res.ErrorCode = ErrorCodeType.PasswordMismatch;
                return res;
            }
        }

        public static RegisterResponse InvalidUserData
        {
            get
            {
                var res = new RegisterResponse();
                res.ErrorCode = ErrorCodeType.InvalidUserData;
                return res;
            }
        }
    }
}