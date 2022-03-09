using KioskStream.Web.Client.Services.Interfaces;

using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;

namespace KioskStream.Web.Client.Shared.Components
{
    public partial class AppBarLoginSection
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IAuthorizationApiAccessor AuthorizationApiAccessor { get; set; }

        private async Task LogoutClick()
        {
            await AuthorizationApiAccessor.Logout();
            NavigationManager.NavigateTo("/account/login");
        }
    }
}
