﻿
using System;
using Microsoft.AspNetCore.Authorization;

namespace KioskStream.Core.Security.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
           // Permission = permission ?? throw new ArgumentNullException(nameof(permission));
        }
    }
}