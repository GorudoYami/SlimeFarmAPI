using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

using SlimeFarmAPI.Game;
using SlimeFarmAPI.Services;

namespace SlimeFarmAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameInfoController : ControllerBase {
        private readonly ILogger<GameInfoController> logger;
        private readonly AccountService accounts;
        private readonly GameInfoService gameInfoService;

        public GameInfoController(ILogger<GameInfoController> logger, AccountService accounts, GameInfoService gameInfoService) {
            this.logger = logger;
            this.accounts = accounts;
            this.gameInfoService = gameInfoService;
            this.gameInfoService.Test();
        }

        [HttpGet("farms")]
        public async Task<ActionResult> GetFarms() {
            var token = HttpContext.Request.Headers["bearer"];
            ulong accountId = accounts.GetIdFromToken(token);
            return Ok(await gameInfoService.GetFarmsAsync(accountId));
        }

        [HttpGet("inventory")]
        public async Task<Inventory> GetInventory() {
            throw new NotImplementedException();
        }
    }
}
