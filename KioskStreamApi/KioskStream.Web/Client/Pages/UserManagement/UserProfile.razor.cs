
using KioskStream.Web.Client.Extensions;

using Microsoft.AspNetCore.Components;

using System;

namespace KioskStream.Web.Client.Pages.UserManagement
{
    public partial class UserProfile
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Parameter] public string UserName { get; set; }

        private bool _isEditEnabled;
        private string _returnUrl;
        private int _tabIndex;
        private bool _deleteUserExpansionOpen;

        protected override void OnInitialized()
        {
            var query = new Uri(NavigationManager.Uri).Query;
            _isEditEnabled = query.GetBoolFromQuery("isEditEnabled");
            _deleteUserExpansionOpen = query.GetBoolFromQuery("deleteUserExpansionOpen");
            _returnUrl = query.GetReturnUrlFromQuery();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                var query = new Uri(NavigationManager.Uri).Query;
                _tabIndex = query.GetNumberFromQuery("tabIndex");
                StateHasChanged();
            }
            base.OnAfterRender(firstRender);
        }
    }
}
