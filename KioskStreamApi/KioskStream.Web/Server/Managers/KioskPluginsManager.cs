using KioskStream.Data;
using KioskStream.Data.Models;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.RequestResponse;
using KioskStream.Web.Common.Utils;
using KioskStream.Web.Server.Managers.Interfaces;
using KioskStream.Web.Server.Wrappers;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KioskStream.Web.Server.Managers
{
    public class KioskPluginsManager : IKioskPluginsManager
    {
        private readonly IRepository<KioskPlugin> _kioskPluginrepository;
        private readonly IRepository<Plugin> _pluginrepository;
        private readonly IRepository<Kiosk> _kioskRepository;

        public KioskPluginsManager(IRepository<KioskPlugin> kioskPluginrepository,
                                   IRepository<Plugin> pluginrepository,
                                   IRepository<Kiosk> kioskRepository)
        {
            _kioskPluginrepository = kioskPluginrepository;
            _pluginrepository = pluginrepository;
            _kioskRepository = kioskRepository;
        }

        public Task<ApiResponse<KioskPluginResponse>> CreateAsync(KioskPluginRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<EmptyResponse>> DeleteAsync(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<KioskPluginResponse>> GetAsync(Guid kioskIdentifier)
        {
            try
            {
                var kioskId = await _kioskRepository.Get(key => key.KioskIdentifier == kioskIdentifier).Where(k => k.Approved).Select(k => k.Id).FirstOrDefaultAsync();
                if (kioskId > 0)
                {
                    var plugin = _pluginrepository.Get(p => !p.KioskPlugins.Any(kp => kp.KioskId == kioskId))
                         .OrderBy(p => p.CreateDateTimeUtc)
                         .FirstOrDefault();

                    KioskPluginResponse kioskPlugin = new KioskPluginResponse { Path = plugin.Path };

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), plugin.Path);
                    if (File.Exists(filePath))
                    {
                        kioskPlugin.File = File.ReadAllBytes(filePath);
                        if (kioskPlugin.File == null || kioskPlugin.File.Length == 0)
                            throw new FileNotFoundException();

                        _kioskPluginrepository.Insert(new KioskPlugin
                        {
                            KioskId = kioskId,
                            PluginId = plugin.Id
                        });
                        await _kioskPluginrepository.SaveChangesAsync();
                    }
                    return new ApiResponse<KioskPluginResponse>(StatusCodes.Status200OK, "Plugin fetched", kioskPlugin);

                }
                return new ApiResponse<KioskPluginResponse>(StatusCodes.Status400BadRequest, "Kiosk is not approved yet.");
            }
            catch (Exception ex)
            {
                // _logger
                return new ApiResponse<KioskPluginResponse>(StatusCodes.Status400BadRequest, "Plugin retrieval failed");
            }
        }

        public Task<ApiResponse<KioskPluginResponse>> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<KioskPluginResponse>>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<EmptyResponse>> UpdateAsync(KioskPluginRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
