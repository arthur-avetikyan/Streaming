
using System;
using KioskStream.Core.Constants.Permissions.Dashboard;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Common.DataTransferObjects.UserManagement;
using KioskStream.Web.Server.Managers.Interfaces;
using KioskStream.Web.Server.Wrappers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace KioskStream.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Get a user based on UserName
        /// </summary>
        /// <returns></returns>
        [HttpGet("{userName}")]
        [Authorize(Dashboard.User.Read)]
        public async Task<ApiResponse<UserDetails>> Get(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            return await _userManager.GetAsync(userName);
        }

        [HttpGet("{userName}/userInfo")]
        public async Task<IActionResult> GetUserInfo(string userName)
        {
            try
            {
                if (userName == null)
                {
                    throw new ArgumentNullException(nameof(userName));
                }

                var result = await _userManager.GetUserInfoAsync(userName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="registerParameters"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Dashboard.User.Create)]
        //public async Task<ApiResponse<EmptyResponse>> Create([FromBody] RegisterParameters registerParameters)
        //{
        //    return await _userManager.CreateAsync(registerParameters);
        //}

        ///// <summary>
        ///// Update a user role
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="userDetails"></param>
        ///// <returns></returns>
        //[HttpPut("{id}")]
        //[Authorize(Dashboard.User.Update)]
        //public async Task<ApiResponse<EmptyResponse>> Update(int id, [FromBody] UserDetails userDetails)
        //{
        //    if (id != userDetails.Id)
        //    {
        //        return new ApiResponse<EmptyResponse>
        //            (StatusCodes.Status400BadRequest, "Update User failed because of ID inconsistency");
        //    }
        //    return await _userManager.UpdateAsync(userDetails);
        //}

        /// <summary>
        /// Change a user password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPut("{userName}/changePassword")]
        [Authorize(Dashboard.User.Update)]
        public async Task<ApiResponse<EmptyResponse>> ChangePassword(string userName, [FromBody] ChangePasswordParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse<EmptyResponse>(StatusCodes.Status400BadRequest, "User Model is Invalid");
            }
            if (userName != parameters.UserName)
            {
                return new ApiResponse<EmptyResponse>
                    (StatusCodes.Status400BadRequest, "Update User failed because of ID inconsistency");
            }
            return await _userManager.ChangePassword(parameters);
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpDelete("{userName}")]
        [Authorize(Dashboard.User.Delete)]
        public async Task<ApiResponse<EmptyResponse>> Delete(string userName)
        {
            return await _userManager.DeleteAsync(userName);
        }
    }
}
