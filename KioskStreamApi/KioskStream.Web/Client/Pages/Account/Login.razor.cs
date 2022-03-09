using KioskStream.Web.Client.Extensions;
using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects.Account;

using MatBlazor;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using System;
using System.Threading.Tasks;

namespace KioskStream.Web.Client.Pages.Account
{
    public partial class Login
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] IAuthorizationApiAccessor AuthorizationApiAccessor { get; set; }
        [Inject] IMatToaster MatToaster { get; set; }

        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        private LoginRequest LoginRequest { get; set; } = new LoginRequest();
        public string ReturnUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var query = new Uri(NavigationManager.Uri).Query;
            ReturnUrl = query.GetReturnUrlFromQuery();

            var user = (await AuthenticationStateTask).User;
            if (user.Identity.IsAuthenticated)
                NavigationManager.NavigateTo("/");
        }

        private async Task SubmitLogin()
        {
            try
            {
                var response = await AuthorizationApiAccessor.Login(LoginRequest);
                if (response.IsSuccessStatusCode)
                {
                    // On successful Login the response.Message is the Last Page Visited from User Profile
                    // We can't navigate yet as the setup is proceeding asynchronously
                    if (!string.IsNullOrEmpty(response.Message))
                    {
                        if (string.IsNullOrWhiteSpace(ReturnUrl))
                            NavigationManager.NavigateTo("/");
                        else
                            NavigationManager.NavigateTo(ReturnUrl);
                    }
                }
                else
                {
                    MatToaster.Add(response.Message, MatToastType.Danger, "Login Attempt Failed");
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "Login Attempt Failed");
            }
        }

        private void Register()
        {
            NavigationManager.NavigateTo("/account/register");
        }
    }
}
