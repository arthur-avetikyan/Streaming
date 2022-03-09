using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.RequestResponse;
using KioskStream.Web.Server.Managers.Interfaces;
using KioskStream.Web.Server.Wrappers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KioskStream.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KiosksController : ControllerBase
    {
        private readonly IKioskManager _kioskManager;

        public KiosksController(IKioskManager kioskManager)
        {
            _kioskManager = kioskManager;
        }

        [HttpGet]
        public async Task<ApiResponse<List<KioskResponse>>> Get([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
        {
            return await _kioskManager.GetListAsync(pageSize, pageNumber);
        }


        [HttpGet("{id}")]

        public async Task<ApiResponse<KioskResponse>> Get(int id) =>
            await _kioskManager.GetAsync(id);

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] KioskRequest request)
        {
            try
            {
                ApiResponse<KioskResponse> result = await _kioskManager.CreateAsync(request);
                return Created(Url.RouteUrl(result.Result.Id), result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<EmptyResponse>>> Put(int id, [FromBody] KioskRequest request)
        {
            if (id != request.Id)
                return new ApiResponse<EmptyResponse>(StatusCodes.Status400BadRequest, "Operation failed because of ID inconsistency");

            if (!ModelState.IsValid)
                return new ApiResponse<EmptyResponse>(StatusCodes.Status400BadRequest, "Model is not valid");

            return await _kioskManager.UpdateAsync(request);
        }
    }
}
