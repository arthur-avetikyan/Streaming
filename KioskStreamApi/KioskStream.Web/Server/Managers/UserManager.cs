
using KioskStream.Core.Enums;
using KioskStream.Core.Exceptions;
using KioskStream.Models;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Common.DataTransferObjects.UserManagement;
using KioskStream.Web.Common.Utils;
using KioskStream.Web.Server.Managers.Interfaces;
using KioskStream.Web.Server.Wrappers;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KioskStream.Data;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.EntityFrameworkCore;

namespace KioskStream.Web.Server.Managers
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<User> _userManager;

        private readonly IRepository<User> _userRepository;

        public UserManager(UserManager<User> userManager, IRepository<User> userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<UserInfo> GetUserInfoAsync(string userName)
        {
            var userInfo = await _userRepository.Get(u => u.UserName == userName)
                                                .Select(u => new UserInfo
                                                {
                                                    Id = u.Id,
                                                    UserName = u.UserName,
                                                    FullName = $"{u.FirstName} {u.LastName}",
                                                })
                                                .FirstOrDefaultAsync();
            return userInfo;
        }

        public async Task<ApiResponse<UserDetails>> GetAsync(string userName)
        {
            var user = _userRepository.Get().FirstOrDefault(u => u.UserName == userName);

            if (user != null)
            {
                try
                {
                    var userDetails = new UserDetails
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Id = user.Id,
                        Roles = await _userManager.GetRolesAsync(user).ConfigureAwait(true) as List<string>
                    };

                    return new ApiResponse<UserDetails>(Status200OK, "User info fetched", userDetails);
                }
                catch (Exception ex)
                {
                    //_logger.LogWarning("Could not build UserInfoDto: " + ex.Message);
                    return new ApiResponse<UserDetails>(Status400BadRequest, "User retrieval failed");
                }
            }
            {
                return new ApiResponse<UserDetails>(Status400BadRequest, "No such user exists!");
            }
        }

        public Task<ApiResponse<UserDetails>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<UserDetails>>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            // get paginated list of users
            try
            {
                //var userList = _userManager.Users.AsQueryable();
                var usersList = _userManager.Users.OrderBy(x => x.Id).Skip(currentPage * pageSize).Take(pageSize).ToList();

                var resultList = new List<UserDetails>();
                foreach (var user in usersList)
                {
                    resultList.Add(new UserDetails
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email,
                        Id = user.Id,
                        Roles = await _userManager.GetRolesAsync(user).ConfigureAwait(true) as List<string>
                    });
                }

                return new ApiResponse<List<UserDetails>>(Status200OK, "User list fetched", resultList);
            }
            catch (Exception ex)
            {
                // _logger
                return new ApiResponse<List<UserDetails>>(Status400BadRequest, "Users list retrieval failed");
            }
        }

        public async Task<ApiResponse<EmptyResponse>> UpdateAsync(UserDetails userDetails)
        {
            var user = await _userManager.FindByIdAsync(userDetails.Id.ToString());

            if (user == null)
            {
                //_logger.LogInformation("User does not exist: {0}", userInfo.Email);
                return new ApiResponse<EmptyResponse>(Status404NotFound, "User does not exist");
            }

            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;
            user.UserName = userDetails.UserName;
            user.Email = userDetails.Email;

            IdentityResult updateResult = await _userManager.UpdateAsync(user);
            IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

            IEnumerable<string> rolesToAdd = userDetails.Roles.Except(roles);
            IEnumerable<string> rolesToRemove = roles.Except(userDetails.Roles);

            IdentityResult addRolesResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
            IdentityResult deleteRolesResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            if (!updateResult.Succeeded || !addRolesResult.Succeeded || !deleteRolesResult.Succeeded)
            {
                //_logger.LogInformation("User Update Failed: {0}", string.Join(",", result.Errors.Select(i => i.Description)));
                return new ApiResponse<EmptyResponse>(Status400BadRequest, "User Update Failed");
            }

            return new ApiResponse<EmptyResponse>(Status200OK, "User Updated Successfully");
        }

        public Task<ApiResponse<EmptyResponse>> UpdateAsync(RegisterParameters request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<EmptyResponse>> DeleteAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ApiResponse<EmptyResponse>(Status404NotFound, "User does not exist");
            }
            try
            {
                await _userManager.DeleteAsync(user);
                return new ApiResponse<EmptyResponse>(Status200OK, "User Deletion Successful");
            }
            catch
            {
                return new ApiResponse<EmptyResponse>(Status400BadRequest, "User Deletion Failed");
            }
        }

        public async Task<ApiResponse<EmptyResponse>> ChangePassword(ChangePasswordParameters parameters)
        {
            User user;

            try
            {
                user = await _userManager.FindByNameAsync(parameters.UserName);
                if (user == null)
                {
                    throw new KeyNotFoundException();
                }

                bool isOldPasswordCorrect = await _userManager.CheckPasswordAsync(user, parameters.OldPassword);
                if (!isOldPasswordCorrect)
                {
                    return new ApiResponse<EmptyResponse>(Status400BadRequest, "The old password is incorrect");
                }
            }
            catch (KeyNotFoundException ex)
            {
                return new ApiResponse<EmptyResponse>(Status400BadRequest, "Unable to find user" + ex.Message);
            }
            try
            {
                var passToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, passToken, parameters.NewPassword);
                if (result.Succeeded)
                {
                    //_logger.LogInformation(user.UserName + "'s password reset; Requested from Admin interface by:" + userClaimsPrincipal.Identity.Name);
                    return new ApiResponse<EmptyResponse>(Status204NoContent, user.UserName + " password reset");
                }
                {
                    //_logger.LogInformation(user.UserName + "'s password reset failed; Requested from Admin interface by:" + userClaimsPrincipal.Identity.Name);

                    // this is going to an authenticated Admin so it should be safe/useful to send back raw error messages
                    if (result.Errors.Any())
                    {
                        return new ApiResponse<EmptyResponse>(Status400BadRequest, string.Join(',', result.Errors.Select(x => x.Description)));
                    }
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex) // not sure if failed password reset result will throw an exception
            {
                //_logger.LogInformation(user.UserName + "'s password reset failed; Requested from Admin interface by:" + userClaimsPrincipal.Identity.Name);
                return new ApiResponse<EmptyResponse>(Status400BadRequest, ex.Message);
            }
        }

        public async Task<ApiResponse<UserDetails>> CreateAsync(RegisterParameters parameters)
        {
            try
            {
                await RegisterNewUserAsync(parameters);
                return new ApiResponse<UserDetails>(Status200OK, "Register User Success");
            }
            catch (DomainException ex)
            {
                return new ApiResponse<UserDetails>(
                    Status400BadRequest,
                    $"Register User Failed: {ex.Description} ");
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserDetails>(Status400BadRequest, ex.GetBaseException().Message);
            }
        }

        private async Task RegisterNewUserAsync(RegisterParameters parameters)
        {
            User user = parameters.Map();
            user.IsActive = false;
            user.CreatedDatetimeUTC = DateTime.UtcNow;
            var createUserResult = await _userManager.CreateAsync(user, parameters.Password);
            if (!createUserResult.Succeeded)
            {
                throw new DomainException(string.Join(",", createUserResult.Errors.Select(i => i.Description)));
            }

            await _userManager.AddToRolesAsync(user, new List<string> { nameof(EAuthorizationRoles.User) });
        }
    }
}
