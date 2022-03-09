using BlazorInputFile;

using KioskStream.Web.Client.Pages.UserManagement;
using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects.Organization;
using KioskStream.Web.Common.DataTransferObjects.RequestResponse;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KioskStream.Web.Client.Pages.KioskManagement
{
    public partial class Plugins
    {
        [Inject] IMatToaster MatToaster { get; set; }
        [Inject] IPluginsApiAccessor PluginsApiAccessor { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        [CascadingParameter] public UserInfo UserInfo { get; set; }

        private int PageSize { get; set; } = 10;
        private int CurrentPage { get; set; } = 0;
        private bool _isUploadDialogOpen = false;
        private PluginRequest PluginRequest { get; set; } = new PluginRequest();

        private List<PluginResponse> _plugins;

        private IMatFileUploadEntry _matFileUpload;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await InitializeKiosksListAsync();
        }

        private async Task InitializeKiosksListAsync()
        {
            try
            {
                var response = await PluginsApiAccessor.GetPluginListAsync(PageSize, CurrentPage);
                if (response.IsSuccessStatusCode)
                {
                    _plugins = response.Result;
                }
                else
                    MatToaster.Add(response.Message + " : " + response.StatusCode, MatToastType.Danger, "Plugins Retrieval Failed");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "Plugins Retrieval Failed");
            }
        }

        private void OpenModal(int id)
        {
            var plugin = _plugins.FirstOrDefault(item => item.Id.Equals(id));

            PluginRequest = new PluginRequest
            {
                Id = plugin.Id,
                Name = plugin.Name
            };
            _isUploadDialogOpen = true;
        }

        private async Task FilesReadyMat(IMatFileUploadEntry[] files)
        {
            var file = files.FirstOrDefault();
            if (file != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    var buffer = new byte[file.Size];

                    await file.WriteToStreamAsync(memoryStream);

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.ReadAsync(buffer);

                    PluginRequest.Plugin = buffer;
                    PluginRequest.FileName = file.Name;
                }
            }
            _matFileUpload = file;
        }

        private async Task Upload()
        {
            try
            {
                var respone = await PluginsApiAccessor.CreatePluginAsync(PluginRequest);
                if (respone.IsSuccessStatusCode)
                {
                    StateHasChanged();
                    _isUploadDialogOpen = false;
                    await OnInitializedAsync();
                }
                //var kiosk = await KiosksApiAccessor.UpdateKioskAsync(KioskRequest);
                ////MatToaster.Add("Employee Created", MatToastType.Success);
                //StateHasChanged();
                //_isEditKioskDialogOpen = false;
                //var item = _kiosks.FirstOrDefault(item => item.Id.Equals(KioskRequest.Id));
                //item.Approved = KioskRequest.Approved;
                //// NavigationManager.NavigateTo($"/kiosks/{employee.Id}?isEditDisabled={true}&returnUrl={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "Plugin upload Error");
            }
        }
    }
}
