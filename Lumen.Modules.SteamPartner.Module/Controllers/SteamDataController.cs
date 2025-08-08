using Lumen.Modules.SteamPartner.Business.Interfaces;
using Lumen.Modules.SteamPartner.Common.Models;

using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace Lumen.Modules.SteamPartner.Module.Controllers {
	[ApiController]
	[Route("[controller]")]
	public class SteamDataController(ILogger<SteamDataController> logger, ISteamPartnerStats steamPartnerStats) : ControllerBase {
		private static DateTime? LastRun = null;

		[HttpPost("queryDataFromSteam")]
		public async Task<IActionResult> QueryDataFromSteam([FromBody] SteamToken steamToken) {
			if (LastRun is not null && LastRun.Value.AddHours(1) > DateTime.UtcNow) {
				logger.LogInformation("Skipping QueryDataFromSteam() to prevent spamming Steam API");
				return NoContent();
			}

			logger.BeginScope("Getting data from the Steam API ...");

			try {
				await steamPartnerStats.QuerySaleAndWishlistDataAsync(steamToken.SteamLoginSecure);

				LastRun = DateTime.UtcNow;

				return Accepted();
			} catch (Exception ex) {
				logger.LogError(ex, "Error when querying steam data");
				return StatusCode((int)HttpStatusCode.InternalServerError);
			}
		}
	}
}
