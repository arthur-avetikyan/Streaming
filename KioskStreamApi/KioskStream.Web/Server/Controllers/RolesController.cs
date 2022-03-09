using System.Collections.Generic;
using System.Threading.Tasks;

using KioskStream.Core.Constants.Permissions;
using KioskStream.Core.Constants.Permissions.Dashboard;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Organization;
using KioskStream.Web.Common.DataTransferObjects.UserManagement;
using KioskStream.Web.Server.Managers;
using KioskStream.Web.Server.Managers.Interfaces;
using KioskStream.Web.Server.Wrappers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace KioskStream.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleManager _roleManager;

        public RolesController(IRoleManager adminManager)
        {
            _roleManager = adminManager;
        }

        /// <summary>
        /// Get all user roles
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Dashboard.Role.Read)]
        public async Task<ApiResponse<List<RoleResponse>>> Get([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
            => await _roleManager.GetListAsync(pageSize, pageNumber);

        /// <summary>
        /// Get user role by name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet("{roleName}")]
        [Authorize(Dashboard.Role.Read)]
        public async Task<ApiResponse<RoleResponse>> Get(string roleName)
            => await _roleManager.GetAsync(roleName);

        /// <summary>
        /// Create a new user role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Authorize(Dashboard.Role.Create)]
        //public async Task<ApiResponse<RoleResponse>> Create([FromBody] RoleRequest role)
        //    => await _roleManager.CreateAsync(role);

        /// <summary>
        /// Update a user role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Dashboard.Role.Update)]
        public async Task<ApiResponse<EmptyResponse>> Update([FromBody] RoleResponse role)
            => await _roleManager.UpdateAsync(role);

        /// <summary>
        /// Delete a user role
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("{roleName}")]
        [Authorize(Dashboard.Role.Delete)]
        public async Task<ApiResponse<EmptyResponse>> Delete(string roleName)
            => await _roleManager.DeleteAsync(roleName);
    }
}
