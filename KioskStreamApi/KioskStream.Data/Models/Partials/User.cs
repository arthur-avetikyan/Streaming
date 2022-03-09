
using KioskStream.Data.Models;

using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;

namespace KioskStream.Models
{
    public partial class User : IdentityUser<int>
    {
        // public virtual int? EmployeeId { get; set; }


      //  public IList<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}