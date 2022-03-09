
using KioskStream.Web.Client.Pages.Account;
using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects.Account;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KioskStream.Web.Client.Shared
{
    public partial class MainLayout
    {
        private bool _navMenuOpened = true;
        private bool _navMinified = false;
        public string bbDrawerClass = "";

        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IUsersApiAccessor UsersApiAccessor { get; set; }

        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        private UserInfo UserInfo { get; set; }

        private ClaimsPrincipal _user;

        protected override async Task OnInitializedAsync()
        {
            _user = (await AuthenticationStateTask).User;
            UserInfo = _user.Identity.IsAuthenticated ? await UsersApiAccessor.GetUserInfoAsync(_user.Identity.Name) : null;
            string absoluteUrl = NavigationManager.Uri;
            List<string> pathsNotToRedirect = new List<string>
            {
                nameof(Register),
                nameof(ConfirmEmail)
            };
            if (!pathsNotToRedirect.Any(path => absoluteUrl.Contains(path, StringComparison.OrdinalIgnoreCase)))
            {
                NavigationManager.NavigateTo(!_user.Identity.IsAuthenticated ? "/account/login" : $"/kiosks");
            }
        }

        private void NavToggle()
        {
            _navMenuOpened = !_navMenuOpened;
            if (_navMenuOpened)
            {
                bbDrawerClass = "full";
            }
            else
            {
                bbDrawerClass = "closed";
            }

            this.StateHasChanged();
        }

        private void NavMinify()
        {
            _navMinified = !_navMinified;

            if (!_navMenuOpened)
            {
                _navMinified = true;
            }

            if (_navMinified)
            {
                bbDrawerClass = "mini";
                _navMenuOpened = true;
            }
            else if (_navMenuOpened)
            {
                bbDrawerClass = "full";
            }

            _navMenuOpened = true;
            this.StateHasChanged();
        }
    }
}
