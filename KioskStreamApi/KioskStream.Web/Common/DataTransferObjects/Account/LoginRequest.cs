﻿
using System.ComponentModel.DataAnnotations;

namespace KioskStream.Web.Common.DataTransferObjects.Account
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
