using KioskStream.Web.Common.DataTransferObjects.RequestResponse;
using KioskStream.Web.Server.Managers.Interfaces;
using KioskStream.Web.Server.Wrappers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KioskStream.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PluginsController : ControllerBase
    {
        private readonly IPluginManager _pluginManager;

        public PluginsController(IPluginManager pluginManager)
        {
            _pluginManager = pluginManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResponse<List<PluginResponse>>> Get([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
        {
            return await _pluginManager.GetListAsync(pageSize, pageNumber);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ApiResponse<PluginResponse>> Get([FromHeader] Guid kioskIdentifier, int id)
        {
            return await _pluginManager.GetAsync(kioskIdentifier, id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PluginRequest request)
        {
            try
            {
                var result = await _pluginManager.CreateAsync(request);
                return Created(Url.RouteUrl(result.Result.Id), result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}