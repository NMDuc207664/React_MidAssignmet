using Library_API_2._0.Application.Dtos.Account.Request;
using Library_API_2._0.Application.Dtos.Users.Request;
using Library_API_2._0.Application.Dtos.Users.Response;
using Library_API_2._0.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_API_2._0.Api.Controller
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDtoResponse>>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPut("{id}/profile")]
        [Authorize]
        public async Task<ActionResult<UserDtoProfileResponse>> UpdateProfile(string id, [FromBody] UserProfileDtoRequest request)
        {
            var user = await _userService.UpdateProfileAsync(request, id);
            return Ok(user);
        }
        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDtoResponse>> UpdateRole(string id, [FromBody] UserRoleDtoRequest request)
        {
            var user = await _userService.UpdateRoleAsync(request, id);
            return Ok(user);
        }
        [HttpPut("{id}/change-password")]
        [Authorize]
        public async Task<ActionResult<UserAccountDataDtoResponse>> ChangePassword(string id, [FromBody] ChangePasswordRequestDto request)
        {
            var user = await _userService.UpdatePasswordAsync(request, id);
            return Ok(user);
        }
        [HttpPut("{id}/reset-password")]
        [Authorize]
        public async Task<ActionResult<UserAccountDataDtoResponse>> ResetPassword(string id)
        {
            var user = await _userService.ResetPasswordAsync(id);
            return Ok(user);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
        [HttpGet("current-user")]
        [Authorize]
        public async Task<ActionResult<UserDtoResponse>> GetCurrentUserByAsync()
        {
            var user = await _userService.GetCurrentUserByAsync();
            return Ok(user);
        }

    }
}