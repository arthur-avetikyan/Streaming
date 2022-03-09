
using System;

namespace KioskStream.Web.Common.DataTransferObjects.Account
{
    public class AuthenticationResult
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
