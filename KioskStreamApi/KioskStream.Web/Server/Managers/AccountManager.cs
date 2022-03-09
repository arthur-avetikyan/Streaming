using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

using KioskStream.Core.Configurations;
using KioskStream.Core.Enums;
using KioskStream.Core.Exceptions;
using KioskStream.Data;
using KioskStream.Data.Models;
using KioskStream.Mailing;
using KioskStream.Models;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Common.Utils;
using KioskStream.Web.Server.Managers.Interfaces;
using KioskStream.Web.Server.Services;
using KioskStream.Web.Server.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Org.BouncyCastle.Security;

using static Microsoft.AspNetCore.Http.StatusCodes;

namespace KioskStream.Web.Server.Managers
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<User> _userManager;

        private readonly IRepository<User> _userRepository;

        private readonly IRepository<RefreshToken> _refreshTokenRepository;

        private readonly IAuthenticationStateManager _authenticationStateManager;

        private readonly ApplicationConfiguration _applicationConfiguration;

        private readonly FileContentProvider _fileContentProvider;

        private readonly IEmailProcessor _emailProcessor;

        private readonly SignInManager<User> _signInManager;

        public AccountManager(
            UserManager<User> userManager,
            IRepository<User> userRepository,
            IRepository<RefreshToken> refreshTokenRepository,
            IAuthenticationStateManager authenticationManager,
            IOptions<ApplicationConfiguration> applicationConfiguration,
            FileContentProvider fileContentProvider,
            IEmailProcessor emailProcessor,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticationStateManager = authenticationManager;
            _fileContentProvider = fileContentProvider;
            _emailProcessor = emailProcessor;
            _signInManager = signInManager;
            _applicationConfiguration = applicationConfiguration.Value;
        }

        public async Task<ApiResponse<AuthenticationResult>> Login(LoginRequest parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.UserName))
            {
                throw new ArgumentNullException(nameof(parameters.UserName));
            }
            var result = await _userRepository
                .Get(u => u.UserName.Equals(parameters.UserName))
                .Select(item =>
                    new
                    {
                        User = item,
                        IsEmailConfirmed = item.EmailConfirmed,
                    })
                .FirstOrDefaultAsync();
            bool isValidLogin = false;
            if (result != null)
            {
                isValidLogin = await _userManager.CheckPasswordAsync(result.User, parameters.Password);
            }
            if (!isValidLogin)
            {
                return
                    new ApiResponse<AuthenticationResult>
                        (Status401Unauthorized, "Username or password are invalid.");
            }

            if (!result.IsEmailConfirmed)
            {
                return
                    new ApiResponse<AuthenticationResult>
                        (Status401Unauthorized, "Your account has not been confirmed, please confirm it first.");
            }

            IEnumerable<Claim> claims = _authenticationStateManager.GetClaims(result.User);
            AuthenticationResult authenticationResult = await _authenticationStateManager.Authenticate(result.User.Id, null, claims);

            return new ApiResponse<AuthenticationResult>(statusCode: Status200OK, message: "Successfully authenticated.", result: authenticationResult);
        }

        public async Task<ApiResponse<RegisterParameters>> Register(RegisterParameters parameters)
        {
            try
            {
                await RegisterNewUserAsync(parameters);
                return new ApiResponse<RegisterParameters>(Status200OK, "Register User Success");
            }
            catch (DomainException ex)
            {
                //_logger.LogError("Register User Failed: {0}, {1}", ex.Description, ex.Message);
                return new ApiResponse<RegisterParameters>(
                    Status400BadRequest,
                    $"Register User Failed: {ex.Description} ");
            }
            //catch (Exception ex)
            //{
            //    _logger.LogError("Register User Failed: {0}", ex.Message);
            //    return new ApiResponse(Status400BadRequest, "Register User Failed");
            //}
        }

        private async Task<User> RegisterNewUserAsync(RegisterParameters parameters)
        {
            User user = parameters.Map();

            var createUserResult = await _userManager.CreateAsync(user, parameters.Password);
            if (!createUserResult.Succeeded)
            {
                throw new DomainException(string.Join(",", createUserResult.Errors.Select(i => i.Description)));
            }

            await _userManager.AddClaimsAsync(user, new[]{
                    new Claim(nameof(EAuthorizationRoles.User), string.Empty),
                    new Claim(nameof(user.UserName), user.UserName),
                    new Claim(nameof(user.Email), user.Email),
                    new Claim(nameof(user.EmailConfirmed), "false", ClaimValueTypes.Boolean)
                });

            //Role - Here we tie the new user to the "User" role
            await _userManager.AddToRolesAsync(user, new List<string> { nameof(EAuthorizationRoles.User) });
            //_logger.LogInformation("New user registered: {0}", user);

            var emailMessage = new EmailConfigurationParameters();

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
            var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
            var callbackUrl = $"{_applicationConfiguration.Url}/Account/{user.UserName}/ConfirmEmail/{token}";

            string emailTemplate = _fileContentProvider.ReadFile(
                Path.Combine(_applicationConfiguration.TemplatesFilePath, "NewUserConfirmationEmail.template.html"));

            var emailMessageBuilderParameters = new ConfirmationEmailParameters
            {
                UserName = user.UserName,
                Recipient = user.Email,
                CallbackUrl = callbackUrl,
                UserId = user.Id.ToString(),
                Token = token,
                Template = emailTemplate
            };

            emailMessage.BuildNewUserConfirmationEmail(emailMessageBuilderParameters); //Replace First UserName with Name if you want to add name to Registration Form

            emailMessage.DestinationAddresses.Add(user.Email);
            try
            {
                await _emailProcessor.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                //_logger.LogInformation("New user email failed: Body: {0}, Error: {1}", emailMessage.Body, ex.Message);
            }

            return user;
        }

        public Task<ApiResponse<ForgotPasswordParameters>> ForgotPassword(ForgotPasswordParameters parameters)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<AuthenticationResult>> Refresh(AuthenticationResult authenticationResult)
        {
            try
            {
                if (authenticationResult is null)
                    return new ApiResponse<AuthenticationResult>(Status403Forbidden, "Invalid operation");

                var authModel = await _refreshTokenRepository
                    .Get(t => t.Revoked == null)
                    .Where(t => t.Expires > DateTime.UtcNow)
                    .Where(w => w.Token.Equals(authenticationResult.RefreshToken))
                    .Select(token =>
                        new
                        {
                            RefrshTokenId = token.Id,
                            token.User,
                        })
                    .FirstOrDefaultAsync();

                if (authModel == null)
                    return new ApiResponse<AuthenticationResult>(Status404NotFound, "User not found");

                IEnumerable<Claim> claims = _authenticationStateManager.GetClaims(authModel.User);
                AuthenticationResult result = await _authenticationStateManager.Authenticate(authModel.User.Id, authModel.RefrshTokenId, claims);

                return new ApiResponse<AuthenticationResult>(statusCode: Status200OK, message: "Successfully authenticated.", result: result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthenticationResult>(Status400BadRequest, ex.GetBaseException().Message);
            }
        }

        public async Task<ApiResponse<string>> Revoke(string userName)
        {
            User user = await _userRepository.Get(u => u.UserName.Equals(userName))
                    .Include(i => i.RefreshTokens)
                    .SingleOrDefaultAsync();

            if (user == null)
                return new ApiResponse<string>(Status404NotFound, "User not found");

            foreach (var token in user.RefreshTokens.Where(x => x.IsActive))
            {
                token.Revoked = DateTime.UtcNow.AddMinutes(-1);
            }
            await _userRepository.SaveChangesAsync();
            return new ApiResponse<string>(Status200OK, "Successful logout");
        }

        public async Task ConfirmEmail(ConfirmEmailRequest confirmEmailRequest)
        {
            if (confirmEmailRequest == null)
            {
                throw new NullReferenceException($"{nameof(confirmEmailRequest)} is null.");
            }

            var user = await _userManager.FindByNameAsync(confirmEmailRequest.UserName);
            if (user == null)
            {
                throw new InvalidParameterException($"User - {nameof(confirmEmailRequest.UserName)} does not exist.");
                //_logger.LogInformation("User does not exist: {0}", parameters.UserId);
                //return new ApiResponse(Status404NotFound, "User does not exist");
            }

            var token = confirmEmailRequest.Token;
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new DomainException("User Email Confirmation Failed: {0}",
                    string.Join(",", result.Errors.Select(i => i.Description)));
                //_logger.LogInformation("User Email Confirmation Failed: {0}", string.Join(",", result.Errors.Select(i => i.Description)));
                //return new ApiResponse(Status400BadRequest, "User Email Confirmation Failed");
            }

            await _signInManager.SignInAsync(user, true);
        }
    }
}