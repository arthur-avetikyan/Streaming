using System;
using System.ComponentModel.DataAnnotations;

using KioskStream.Models;

using Microsoft.EntityFrameworkCore;

namespace KioskStream.Data.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => Revoked == null && !IsExpired;

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
