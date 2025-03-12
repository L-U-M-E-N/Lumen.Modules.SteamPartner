using Lumen.Modules.SteamPartner.Business.Interfaces;
using Lumen.Modules.SteamPartner.Common.Models;

using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace Lumen.Modules.SteamPartner.Module.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class SteamDataController(ILogger<SteamDataController> logger, ISteamPartnerStats steamPartnerStats) : ControllerBase {
        [HttpPost("queryDataFromSteam")]
        public async Task<IActionResult> QueryDataFromSteam([FromBody] SteamToken steamToken) {
            logger.BeginScope("Getting data from the Steam API ...");

            try {
                await steamPartnerStats.QuerySaleAndWishlistDataAsync(steamToken.SteamLoginSecure);

                return NoContent();
            } catch (Exception ex) {
                logger.LogError(ex, "Error when querying steam data");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}