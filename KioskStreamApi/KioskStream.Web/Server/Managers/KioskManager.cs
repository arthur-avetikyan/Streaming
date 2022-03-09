using System;
using System.Collections.Generic;
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
    public class KioskManager : IKioskManager
    {
        private readonly IRepository<Kiosk> _repository;

        public KioskManager(IRepository<Kiosk> repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<KioskResponse>> GetAsync(int key)
        {
            try
            {
                var result = await _repository.Get(item => item.Id.Equals(key)).FirstOrDefaultAsync();
                return new ApiResponse<KioskResponse>(Status200OK, "Get kiosk fetched", result);
            }
            catch (Exception ex)
            {
                // _logger
                return new ApiResponse<KioskResponse>(Status400BadRequest, "Kiosks list retrieval failed");
            }
        }

        public async Task<ApiResponse<List<KioskResponse>>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            try
            {
                var result = await _repository.Get().OrderBy(x => x.Id).Skip(currentPage * pageSize).Take(pageSize)
                    .ToListAsync();
                return new ApiResponse<List<KioskResponse>>(Status200OK, "Get kiosk fetched", result.Select(item => item.Map()).ToList());
            }
            catch (Exception ex)
            {
                // _logger
                return new ApiResponse<List<KioskResponse>>(Status400BadRequest, "Kiosks list retrieval failed");
            }
        }

        public async Task<ApiResponse<KioskResponse>> CreateAsync(KioskRequest request)
        {
            var kiosk = request.Map(new Kiosk());
            kiosk.CreateDateTimeUtc = DateTime.UtcNow;
            _repository.Insert(kiosk);
            await _repository.SaveChangesAsync();
            var response = kiosk.Map();

            return new ApiResponse<KioskResponse>(Status200OK, "Kiosks created", response);
        }

        public Task<ApiResponse<EmptyResponse>> UpdateAsync(KioskResponse response)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<EmptyResponse>> UpdateAsync(KioskRequest request)
        {
            var kiosk = await _repository.Get(item => item.Id.Equals(request.Id)).FirstOrDefaultAsync();
            kiosk = request.Map(kiosk);
            _repository.Update(kiosk);
            await _repository.SaveChangesAsync();
            var response = kiosk.Map();

            return new ApiResponse<EmptyResponse>(Status200OK, "Kiosks updated", response);
        }

        public Task<ApiResponse<EmptyResponse>> DeleteAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
