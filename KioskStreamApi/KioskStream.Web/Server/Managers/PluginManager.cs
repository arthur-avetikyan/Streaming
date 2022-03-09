using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using KioskStream.Data;
using KioskStream.Data.Models;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.RequestResponse;
using KioskStream.Web.Common.Utils;
using KioskStream.Web.Server.Managers.Interfaces;
using KioskStream.Web.Server.Wrappers;

using Microsoft.EntityFrameworkCore;

using static Microsoft.AspNetCore.Http.StatusCodes;

namespace KioskStream.Web.Server.Managers
{
    public class PluginManager : IPluginManager
    {
        private readonly IRepository<Plugin> _repository;
        private readonly IRepository<Kiosk> _kioskRepository;

        public PluginManager(IRepository<Plugin> repository, IRepository<Kiosk> kioskRepository)
        {
            _repository = repository;
            _kioskRepository = kioskRepository;
        }

        public async Task<ApiResponse<PluginResponse>> GetAsync(Guid kioskIdentifier, int key)
        {
            try
            {
                if (_kioskRepository.Get(key => key.KioskIdentifier == kioskIdentifier).Any(k => k.Approved))
                {
                    var result = await _repository.Get(item => item.Id.Equals(key)).FirstOrDefaultAsync();
                    var response = result.Map();

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), result.Path);
                    if (File.Exists(filePath))
                    {
                        response.File = File.ReadAllBytes(filePath);
                    }
                    return new ApiResponse<PluginResponse>(Status200OK, "Get Plugin fetched", response);

                }
                return new ApiResponse<PluginResponse>(Status400BadRequest, "Kiosk is not approved yet.");
            }
            catch (Exception ex)
            {
                // _logger
                return new ApiResponse<PluginResponse>(Status400BadRequest, "Plugin retrieval failed");
            }
        }

        public async Task<ApiResponse<List<PluginResponse>>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            try
            {
                var result = await _repository.Get()
                                              .OrderBy(x => x.Id)
                                              .Skip(currentPage * pageSize)
                                              .Take(pageSize)
                                              .ToListAsync();
                return new ApiResponse<List<PluginResponse>>(Status200OK, "Get Plugin fetched", result.Select(item => item.Map()).ToList());
            }
            catch (Exception ex)
            {
                // _logger
                return new ApiResponse<List<PluginResponse>>(Status400BadRequest, "Plugin list retrieval failed");
            }
        }

        public async Task<ApiResponse<PluginResponse>> CreateAsync(PluginRequest request)
        {
            var plugin = await request.Map(new Plugin());
            _repository.Insert(plugin);
            await _repository.SaveChangesAsync();
            var response = plugin.Map();

            return new ApiResponse<PluginResponse>(Status200OK, "Plugin created", response);
        }

        public Task<ApiResponse<EmptyResponse>> UpdateAsync(PluginRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<EmptyResponse>> DeleteAsync(int key)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<PluginResponse>> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
