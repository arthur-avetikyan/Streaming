
using KioskStream.Data.Models;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;

namespace KioskStream.Models
{
    public partial class User : IdentityUser<int>
    {
        // public virtual int? EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDatetimeUTC { get; set; }
        public DateTime? UpdatedDatetimeUTC { get; set; }

        public IList<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}