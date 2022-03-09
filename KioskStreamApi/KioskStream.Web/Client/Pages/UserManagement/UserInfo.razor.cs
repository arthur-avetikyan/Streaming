
using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Common.DataTransferObjects.UserManagement;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KioskStream.Web.Client.Pages.UserManagement
{
    public partial class UserInfo
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private IUsersApiAccessor UsersApiAccessor { get; set; }
        [Inject] private IRoleApiAccessor RolesApiAccessor { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Parameter] public string UserName { get; set; }
        [Parameter] public bool IsEditEnabled { get; set; } = false;

        private const int RolesPopulationCount = 100;

        private bool _changePasswordDialogOpen = false;
        private UserDetails _tempUserDetails;

        private UserDetails UserDetails { get; set; } = new UserDetails();
        private ChangePasswordParameters ChangePasswordParameters { get; set; } = new ChangePasswordParameters();
        private List<UserRoleStatus> UserRoleStatuses { get; set; } = new List<UserRoleStatus>();

        protected override async Task OnInitializedAsync()
        {
            await RetrieveUserAsync();
            await RetrieveRolesAsync();

            if (UserDetails != null)
            {
                _tempUserDetails = UserDetails.Clone();
            }
        }

        private async Task RetrieveUserAsync()
        {
            try
            {
                var apiResponse = await UsersApiAccessor.GetAsync(UserName);

                if (apiResponse.IsSuccessStatusCode)
                {
                    MatToaster.Add(apiResponse.Message, MatToastType.Success, "Users Retrieved");
                    UserDetails = apiResponse.Result;
                }
                else
                    MatToaster.Add(apiResponse.Message + " : " + apiResponse.StatusCode, MatToastType.Danger, "User Retrieval Failed");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "User Retrieval Error");
            }
        }

        private async Task RetrieveRolesAsync()
        {
            try
            {
                var roles = new List<RoleResponse>();

                ApiResponseDetails<List<RoleResponse>> lApiResponse =
                    await RolesApiAccessor.GetRolesAsync(RolesPopulationCount);
                if (lApiResponse.IsSuccessStatusCode)
                {
                    MatToaster.Add("Success", MatToastType.Success, lApiResponse.Message);
                    roles = lApiResponse.Result;
                }
                else
                {
                    MatToaster.Add(
                        lApiResponse.Message + " : " + lApiResponse.StatusCode,
                        MatToastType.Danger,
                        "Roles Retrieval Failed");
                }

                UserRoleStatuses = new List<UserRoleStatus>();// clear out list

                // initialize selection list with all un-selected
                foreach (var role in roles)
                {
                    UserRoleStatuses.Add(new UserRoleStatus
                    {
                        Name = role.Name,
                        IsSelected = false
                    });
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "Role Retrieval Error");
            }
        }

        private void OpenChangePasswordDialog()
        {
            ChangePasswordParameters = new ChangePasswordParameters
            {
                UserName = UserDetails.UserName
            };
            _changePasswordDialogOpen = true;
        }

        private async Task ChangeUserPasswordAsync()
        {
            try
            {
                if (ChangePasswordParameters.NewPassword != ChangePasswordParameters.PasswordConfirm)
                {
                    MatToaster.Add("Passwords Must Match", MatToastType.Warning);
                }
                else
                {
                    var apiResponse = await UsersApiAccessor.ChangePasswordAsync(ChangePasswordParameters);

                    if (apiResponse.IsSuccessStatusCode)
                        MatToaster.Add("Password changed", MatToastType.Success, apiResponse.Message);
                    else
                        MatToaster.Add(apiResponse.Message, MatToastType.Danger);

                    _changePasswordDialogOpen = false;
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "Password Reset Error");
            }
        }

        private void EnableEdit()
        {
            foreach (var role in UserRoleStatuses)
            {
                if (UserDetails != null)
                {
                    role.IsSelected = UserDetails.Roles.Contains(role.Name);
                }
            }
            IsEditEnabled = true;
        }

        private void DisableEdit()
        {
            UserDetails = _tempUserDetails.Clone();
            IsEditEnabled = false;
        }

        private async Task UpdateAsync()
        {
            try
            {
                //update the user object's role list with the new selection set
                UserDetails.Roles = UserRoleStatuses.Where(x => x.IsSelected == true).Select(x => x.Name).ToList();

                var apiResponse = await UsersApiAccessor.UpdateAsync(UserDetails);

                if (apiResponse.IsSuccessStatusCode)
                {
                    MatToaster.Add("User Updated", MatToastType.Success);
                    NavigationManager.NavigateTo($"users/{UserDetails.UserName}");
                }
                else
                {
                    MatToaster.Add("Error", MatToastType.Danger, apiResponse.StatusCode.ToString());
                    //user.RestoreState();
                }

                IsEditEnabled = false;
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "User Update Error");
            }
            finally
            {
                //user.ClearState();
            }
        }

        private void UpdateUserRole(UserRoleStatus userRoleStatus)
        {
            userRoleStatus.IsSelected = !userRoleStatus.IsSelected;
        }
    }
}
