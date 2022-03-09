using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Organization;
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
    public class KioskPluginsController : ControllerBase
    {
        private readonly IKioskPluginsManager _kioskPluginsManager;

        public KioskPluginsController(IKioskPluginsManager kioskPluginsManager)
        {
            _kioskPluginsManager = kioskPluginsManager;
        }

        [HttpGet()]
        [AllowAnonymous]
        public async Task<ApiResponse<KioskPluginResponse>> Get([FromHeader] Guid kioskIdentifier) =>
            await _kioskPluginsManager.GetAsync(kioskIdentifier);
    }
}
