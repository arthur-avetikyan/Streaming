
using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Common.DataTransferObjects.UserManagement;

using MatBlazor;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace KioskStream.Web.Client.Pages.UserManagement
{
    public partial class Users
    {
        //[Inject]
        //private HttpClient HttpClient { get; set; }

        //[Inject]
        //private IAuthorizationService AuthorizationService { get; set; }

        //[Inject]
        //private AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        private IMatToaster MatToaster { get; set; }

        [Inject]
        private IUsersApiAccessor UsersApiAccessor { get; set; }

        [Inject]
        private IRoleApiAccessor RolesApiAccessor { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private int PageSize { get; set; } = 10;

        private int CurrentPage { get; set; } = 0;

        private bool _createUserDialogOpen = false;
        
        private bool _changePasswordDialogOpen = false;

        private List<UserDetails> _users;

        private List<UserRoleStatus> UserRoleStatuses { get; set; } = new List<UserRoleStatus>();

        private UserDetails User { get; set; } = new UserDetails();
        
        private RegisterParameters RegisterParameters { get; set; } = new RegisterParameters();
        
        //UserProfileDto userProfile = new UserProfileDto();

        private ChangePasswordParameters ChangePasswordParameters { get; set; } = new ChangePasswordParameters();

        private const int RolesPopulationCount = 100;

        private bool _collapsed = true;

        private UserDetails _currentSelectedUser;

        private string _currentUrl;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await RetrieveUsersAsync();
            await RetrieveRolesAsync();
            
            _currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        }

        private void SelectionChangedEvent(object row)
        {
            if (row != null)
            {
                _currentSelectedUser = (UserDetails)row;
            }

            NavigationManager.NavigateTo($"users/{_currentSelectedUser.UserName}?returnUrl={_currentUrl}");
        }

        private async Task RetrieveUsersAsync()
        {
            try
            {
                var apiResponse = await UsersApiAccessor.GetListAsync();

                if (apiResponse.IsSuccessStatusCode)
                {
                    MatToaster.Add(apiResponse.Message, MatToastType.Success, "Users Retrieved");
                    _users = apiResponse.Result;
                }
                else
                    MatToaster.Add(apiResponse.Message + " : " + apiResponse.StatusCode, MatToastType.Danger, "User Retrieval Failed");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "User Retrieval Error");
            }
        }

        private void OpenEditDialog(string userName)
        {
            NavigationManager.NavigateTo($"/users/{userName}?isEditEnabled=true&returnUrl={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}");
        }

        private void OpenChangePasswordDialog(string userName, int userId)
        {
            ChangePasswordParameters = new ChangePasswordParameters
            {
                UserName = userName
            };
            User.Id = userId;
            _changePasswordDialogOpen = true;
        }

        private void OpenDeleteDialog(string userName)
        {
            NavigationManager.NavigateTo($"/users/{userName}?tabIndex=1&deleteUserExpansionOpen=true&returnUrl={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}");
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

        private async Task CreateUserAsync()
        {
            try
            {
                if (RegisterParameters.Password != RegisterParameters.PasswordConfirm)
                {
                    MatToaster.Add("Password Confirmation Failed", MatToastType.Danger, "");
                    return;
                }

                var apiResponse = await UsersApiAccessor.CreateAsync(RegisterParameters);
                if (apiResponse.IsSuccessStatusCode)
                {
                    MatToaster.Add(apiResponse.Message, MatToastType.Success);
                    //users.Add(registerParameters.Map());
                    RegisterParameters = new RegisterParameters(); //reset create user object after insert
                    _createUserDialogOpen = false;
                    StateHasChanged();
                    await OnInitializedAsync();
                }
                else
                {
                    MatToaster.Add(apiResponse.Message + " : " + apiResponse.StatusCode, MatToastType.Danger, "User Creation Failed");
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "User Creation Error");
            }
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

        private void ShowMore(int userId)
        {
            var user = _users.FirstOrDefault(r => r.Id == userId);
            user.DisplayMore = !user.DisplayMore;
            StateHasChanged();
        }
    }
}
