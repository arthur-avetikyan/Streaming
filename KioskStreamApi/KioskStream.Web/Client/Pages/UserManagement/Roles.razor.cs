using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.UserManagement;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KioskStream.Web.Client.Pages.UserManagement
{
    public partial class Roles
    {
        [Inject] IRoleApiAccessor RoleApiAccessor { get; set; }
        [Inject] IMatToaster MatToaster { get; set; }
        private int PageSize { get; set; } = 10;
        private int CurrentPage { get; set; } = 0;

        private int _currentRoleId = 0;
        private string _currentRoleName = "";
        private bool _isCurrentRoleReadOnly = false;
        private bool _isDeleteDialogOpen = false;
        private bool _isUpsertDialogOpen = false;
        private List<PermissionSelection> _permissionsSelections = new List<PermissionSelection>();

        private bool _isInsertOperation;
        private string _labelUpsertDialogTitle;
        private List<RoleResponse> _roles;

        public class PermissionSelection
        {
            public bool IsSelected { get; set; }
            public string Name { get; set; }
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await InitializeRolesListAsync();
            SortData(null);
        }

        private async Task InitializeRolesListAsync()
        {
            try
            {
                ApiResponseDetails<List<RoleResponse>> lApiResponse = await RoleApiAccessor.GetRolesAsync(PageSize, CurrentPage);
                if (lApiResponse.IsSuccessStatusCode)
                {
                    MatToaster.Add("Success", MatToastType.Success, lApiResponse.Message);
                    _roles = lApiResponse.Result;
                }
                else
                    MatToaster.Add(lApiResponse.Message + " : " + lApiResponse.StatusCode, MatToastType.Danger, "Roles Retrieval Failed");

            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "Roles Retrieval Error");
            }
        }

        private void SortData(MatSortChangedEvent sort)
        {
            if (!(sort == null || sort.Direction == MatSortDirection.None || string.IsNullOrEmpty(sort.SortId)))
            {
                Comparison<RoleResponse> comparison = null;
                switch (sort.SortId)
                {
                    case "role":
                        comparison = (s1, s2) => string.Compare(s1.Name, s2.Name, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "permissions":
                        comparison = (s1, s2) => s1.Permissions.Count.CompareTo(s2.Permissions.Count);
                        break;
                }
                if (comparison != null)
                {
                    if (sort.Direction == MatSortDirection.Desc)
                        _roles.Sort((s1, s2) => -1 * comparison(s1, s2));
                    else
                        _roles.Sort(comparison);
                }
            }
        }

        private async Task OpenUpsertRoleDialog(int roleId = 0, string roleName = "")
        {
            try
            {
                _currentRoleName = roleName;
                _currentRoleId = roleId;

                _isInsertOperation = string.IsNullOrWhiteSpace(roleName) && roleId == 0;

                if (_isInsertOperation)
                    _labelUpsertDialogTitle = "Create Role";
                else
                    _labelUpsertDialogTitle = "Edit Role";

                ApiResponseDetails<RoleResponse> role = null;
                if (!_isInsertOperation)
                {
                    role = await RoleApiAccessor.GetRoleAsync(_currentRoleName);
                }

                ApiResponseDetails<List<string>> premissions = await RoleApiAccessor.GetPremissions();
                _permissionsSelections.Clear();
                foreach (var name in premissions.Result)
                {
                    var newPermissionsSelection = new PermissionSelection
                    {
                        Name = name,
                        IsSelected = role != null && role.Result.Permissions.Contains(name)
                    };
                    _permissionsSelections.Add(newPermissionsSelection);
                }
                _isUpsertDialogOpen = true;
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, $"{_labelUpsertDialogTitle} Error");
            }
        }

        private void OpenDeleteDialog(string roleName)
        {
            _currentRoleName = roleName;
            _isDeleteDialogOpen = true;
        }

        private async Task UpsertRole()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_currentRoleName) && _currentRoleId > 0)
                {
                    MatToaster.Add("Role Creation Error", MatToastType.Danger, "Enter in a Role Name");
                    return;
                }

                RoleResponse request = new RoleResponse
                {
                    Id = _currentRoleId,
                    Name = _currentRoleName,
                    Permissions = new List<string>()
                };

                foreach (var status in _permissionsSelections)
                    if (status.IsSelected)
                        request.Permissions.Add(status.Name);

                ApiResponseDetails<EmptyResponse> lApiResponseDetails = null;

                if (_isInsertOperation)
                    lApiResponseDetails = await RoleApiAccessor.InsertRoleAsync(request);
                else
                    lApiResponseDetails = await RoleApiAccessor.UpdateRoleAsync(request);

                if (lApiResponseDetails.IsSuccessStatusCode)
                {
                    MatToaster.Add(_isInsertOperation ? "Role Created" : "Role Updated", MatToastType.Success);
                    StateHasChanged();
                }
                else
                    MatToaster.Add(lApiResponseDetails.Message, MatToastType.Danger);

                await OnInitializedAsync();
                _isUpsertDialogOpen = false;
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "Role Operation Error");
            }
        }

        private async Task DeleteRoleAsync()
        {
            try
            {
                var response = await RoleApiAccessor.DeleteRoleAsync(_currentRoleName);
                if (response.IsSuccessStatusCode)
                {
                    MatToaster.Add("Role Delete Failed", MatToastType.Danger);
                    return;
                }

                MatToaster.Add("Role Deleted", MatToastType.Success);
                await OnInitializedAsync();
                _isDeleteDialogOpen = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "Role Delete Error");
            }
        }

        private void ShowMore(int roleId)
        {
            var role = _roles.FirstOrDefault(r => r.Id == roleId);
            role.DisplayMore = !role.DisplayMore;
            StateHasChanged();
        }
    }
}
