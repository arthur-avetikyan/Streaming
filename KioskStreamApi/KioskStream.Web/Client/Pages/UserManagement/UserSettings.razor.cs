
using System;
using System.Threading.Tasks;
using KioskStream.Core.Constants.Permissions.Dashboard;
using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Common.DataTransferObjects.UserManagement;
using MatBlazor;
using Microsoft.AspNetCore.Components;

namespace KioskStream.Web.Client.Pages.UserManagement
{
    public partial class UserSettings
    {
        [Inject]
        private IMatToaster MatToaster { get; set; }

        [Inject]
        private IUsersApiAccessor UsersApiAccessor { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string UserName { get; set; }

        private ChangePasswordParameters ChangePasswordParameters { get; set; } = new ChangePasswordParameters();

        private bool _changePasswordExpansionOpen = false;

        [Parameter]
        public bool DeleteUserExpansionOpen { get; set; }

        private MatTheme _redTheme = new MatTheme()
        {
            Primary = MatThemeColors.Red._500.Value,
            Secondary = MatThemeColors.Red._300.Value
        };

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

                    _changePasswordExpansionOpen = false;
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "Password Reset Error");
            }
        }

        private async Task DeleteUserAsync()
        {
            try
            {
                var apiResponse = await UsersApiAccessor.DeleteAsync(UserName);

                if (apiResponse.IsSuccessStatusCode)
                {
                    MatToaster.Add("User Deleted", MatToastType.Success);
                    DeleteUserExpansionOpen = false;
                    StateHasChanged();
                    NavigationManager.NavigateTo("/users");
                }
                else
                    MatToaster.Add("User Delete Failed", MatToastType.Danger);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "User Delete Error");
            }
        }
    }
}
