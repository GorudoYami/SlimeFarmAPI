using System;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SlimeFarmAPI.DTOs;
using SlimeFarmAPI.Services;

namespace SlimeFarmAPI.Controllers {
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase {
        private readonly ILogger<AccountController> logger;
        private readonly AccountService accounts;

        public AccountController(ILogger<AccountController> logger, AccountService accounts) {
            this.logger = logger;
            this.accounts = accounts;
        }

        // Login with email doesnt work
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginDTO loginDTO) {
            logger.LogInformation("User tries to log in...");
            var token = await accounts.LoginAsync(loginDTO);
            if (token == null) {
                logger.LogInformation("User doesn't exist");
                return NotFound();
            }
            else {
                logger.LogInformation("User logged in. Token has been sent.");
                return Ok(token);
            }
        }

        [HttpPost("refresh")]
        public ActionResult Refresh([FromBody] string token) {
            logger.LogInformation("Token refresh requested.");
            return Ok(accounts.Refresh(token));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(AccountDTO accountDTO) {
            if (await accounts.RegisterAsync(accountDTO) == null) {
                logger.LogInformation("A new account has been created.");
                return Ok();
            }
            else {
                logger.LogInformation("There was an attempt to create an acoount but a similar one already exists.");
                return Conflict();
            }
        }

        [HttpPost("changepassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO) {
            logger.LogInformation("User tries to change password...");
            if (await accounts.ChangePasswordAsync(changePasswordDTO))
                return Ok();
            else
                return Conflict();
            }
        }
    }
}
