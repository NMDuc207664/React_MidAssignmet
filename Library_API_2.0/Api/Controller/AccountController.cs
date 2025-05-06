
using Library_API_2._0.Application.Dtos.Users.Request;
using Library_API_2._0.Application.Interface;

using Microsoft.AspNetCore.Mvc;

namespace Library_API_2._0.Api.Controller
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationRequestDto request)
        {
            if (request == null) return BadRequest();

            var result = await _accountService.RegisterAsync(request);
            if (result.Errors != null && result.Errors.Any())
            {
                return BadRequest(result);
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] UserAuthRequestDto request)
        {
            var result = await _accountService.AuthenticateAsync(request);
            if (!result.IsAuthSucessful)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}