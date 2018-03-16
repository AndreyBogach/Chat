using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Models
{
    public class LoginResponse
    {
        public LoginResponse() { }

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }

        public ErrorCodeType ErrorCode { get; set; }

        public enum ErrorCodeType
        {
            None = 0,
            InvalidUserData = 1,
            EmailNotFound = 2,
            InvalidCredential = 3,
            TrialExpired = 4,
            UnknowError = 0xFF
        }

        public bool Success
        {
            get { return ErrorCode == ErrorCodeType.None; }
        }

        public static LoginResponse TrialExpired
        {
            get
            {
                var res = new LoginResponse();
                res.ErrorCode = ErrorCodeType.TrialExpired;
                return res;
            }
        }

        public static LoginResponse InvalidUserData
        {
            get
            {
                var res = new LoginResponse();
                res.ErrorCode = ErrorCodeType.InvalidUserData;
                return res;
            }
        }

        public static LoginResponse EmailNotFound
        {
            get
            {
                var res = new LoginResponse();
                res.ErrorCode = ErrorCodeType.EmailNotFound;
                return res;
            }
        }

        public static LoginResponse InvalidCredential
        {
            get
            {
                var res = new LoginResponse();
                res.ErrorCode = ErrorCodeType.InvalidCredential;
                return res;
            }
        }

        public static LoginResponse UnknowError
        {
            get
            {
                var res = new LoginResponse();
                res.ErrorCode = ErrorCodeType.UnknowError;
                return res;
            }
        }
    }
}