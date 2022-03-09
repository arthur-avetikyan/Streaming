using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Common.DataTransferObjects.RequestResponse;
using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KioskStream.Web.Client.Pages.KioskManagement
{
    public partial class Kiosks
    {
        [Inject] IMatToaster MatToaster { get; set; }
        [Inject] IKiosksApiAccessor KiosksApiAccessor { get; set; }

        private int PageSize { get; set; } = 10;
        private int CurrentPage { get; set; } = 0;
        private bool _isEditKioskDialogOpen = false;
        private KioskRequest KioskRequest { get; set; } = new KioskRequest();

        private List<KioskResponse> _kiosks;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await InitializeKiosksListAsync();
        }

        private async Task InitializeKiosksListAsync()
        {
            try
            {
                var response = await KiosksApiAccessor.GetKiosksListAsync(PageSize, CurrentPage);
                if (response.IsSuccessStatusCode)
                {
                    _kiosks = response.Result;
                }
                else
                    MatToaster.Add(response.Message + " : " + response.StatusCode, MatToastType.Danger, "Kiosks Retrieval Failed");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "Kiosks Retrieval Error");
            }
        }

        private void OpenModal(int id)
        {
            _isEditKioskDialogOpen = true;
            var kiosk =  _kiosks.FirstOrDefault(item => item.Id.Equals(id));

            KioskRequest = new KioskRequest
            {
                Id = kiosk.Id,
                Name = kiosk.Name,
                TimeZone = kiosk.TimeZone,
                Location = kiosk.Location,
                KioskIdentifier = kiosk.KioskIdentifier,
                Approved = kiosk.Approved
            };
        }

        private async Task Approve()
        {
            try
            {
                var kiosk = await KiosksApiAccessor.UpdateKioskAsync(KioskRequest);
                //MatToaster.Add("Employee Created", MatToastType.Success);
                StateHasChanged();
                _isEditKioskDialogOpen = false;
                var item =  _kiosks.FirstOrDefault(item => item.Id.Equals(KioskRequest.Id));
                item.Approved = KioskRequest.Approved;
                // NavigationManager.NavigateTo($"/kiosks/{employee.Id}?isEditDisabled={true}&returnUrl={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "Kiosk update Error");
            }
        }
    }
}
