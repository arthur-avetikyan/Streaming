using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects.Account;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KioskStream.Web.Client.Pages.Account
{
    public partial class ConfirmEmail
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private HttpClient HttpClient { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        private IMatToaster MatToaster { get; set; }

        [Inject]
        public IAuthorizationApiAccessor AuthorizationApiAccessor { get; set; }

        private ConfirmEmailRequest ConfirmEmailRequest { get; set; } = new ConfirmEmailRequest();

        bool disableConfirmButton = false;

        [Parameter]
        public string UserName { get; set; }

        [Parameter]
        public string Token { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var absoluteUrl = NavigationManager.Uri;

            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Token))
            {
                disableConfirmButton = true;
                ConfirmEmailRequest = new ConfirmEmailRequest
                {
                    Token = Token,
                    UserName = UserName
                };
                await SendConfirmation();
            }
        }

        async Task SendConfirmation()
        {
            bool result = await AuthorizationApiAccessor.ConfirmEmailAsync(ConfirmEmailRequest);
            if (result)
            {
                MatToaster.Add("Account has been Approved and Activated", MatToastType.Success);
                NavigationManager.NavigateTo("/");
            }
            else
            {
                MatToaster.Add("Email Verification Failed", MatToastType.Danger);

            }
        }
    }
}
