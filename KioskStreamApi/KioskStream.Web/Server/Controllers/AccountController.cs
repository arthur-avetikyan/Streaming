
using System;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Server.Managers.Interfaces;
using KioskStream.Web.Server.Wrappers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using KioskStream.Core.Exceptions;
using Org.BouncyCastle.Security;

namespace KioskStream.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        //private readonly ApiResponse<UserDetails> _invalidUserModel;

        public AccountController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
            //_invalidUserModel = new ApiResponse<UserDetails>(StatusCodes.Status400BadRequest, "User model is invalid");
        }

        [HttpPost("register")]
        public async Task<ApiResponse<RegisterParameters>> Register(RegisterParameters parameters)
            => ModelState.IsValid ?
                await _accountManager.Register(parameters)
                : new ApiResponse<RegisterParameters>(statusCode: StatusCodes.Status400BadRequest, message: "User model is invalid");

        [HttpPost("confirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> Confirm(ConfirmEmailRequest confirmEmailRequest)
        {
            try
            {
                await _accountManager.ConfirmEmail(confirmEmailRequest);
                return Ok();
            }
            catch (NullReferenceException e)
            {
                return StatusCode(404);
            }
            catch (InvalidParameterException e)
            {
                return StatusCode(404);
            }
            catch (DomainException e)
            {
                return StatusCode(400);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ApiResponse<AuthenticationResult>> Login(LoginRequest parameters)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse<AuthenticationResult>(StatusCodes.Status400BadRequest, "User model is invalid");
            }

            try
            {
                return await _accountManager.Login(parameters);
            }
            catch (Exception e)
            {
                return new ApiResponse<AuthenticationResult>(StatusCodes.Status400BadRequest, "Invalid login attempt");
            }
        }

        [HttpPost("refreshToken")]
        [AllowAnonymous]
        public async Task<ApiResponse<AuthenticationResult>> RefreshToken(AuthenticationResult authenticationRequest)
            => await _accountManager.Refresh(authenticationRequest);

        [HttpPost("logout")]
        [Authorize]
        public async Task<ApiResponse<string>> Logout()
            => await _accountManager.Revoke(User.Identity.Name);
    }
}
