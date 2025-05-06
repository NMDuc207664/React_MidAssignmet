using AutoMapper;
using Library_API_2._0.Application.Dtos.Account.Request;
using Library_API_2._0.Application.Dtos.Users.Request;
using Library_API_2._0.Application.Dtos.Users.Response;
using Library_API_2._0.Application.Interface;
using Library_API_2._0.Domain.Entities;
using Library_API_2._0.Domain.Factories;
using Microsoft.AspNetCore.Identity;

namespace Library_API_2._0.Application.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(UserManager<User> userManager, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new Exception("User not found");
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception("Failed to delete user");
        }

        public async Task<IEnumerable<UserDtoResponse>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var result = new List<UserDtoResponse>();
            foreach (var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);
                var dto = _mapper.Map<UserDtoResponse>(user);
                dto.Role = role.FirstOrDefault();
                result.Add(dto);
            }
            return result;
        }

        public async Task<UserDtoResponse> GetCurrentUserByAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
                throw new UnauthorizedAccessException("User not authenticated");

            var roles = await _userManager.GetRolesAsync(user);
            var dto = _mapper.Map<UserDtoResponse>(user);
            dto.Role = roles.FirstOrDefault();

            return dto;
        }

        public async Task<UserDtoProfileResponse> GetUserByIdAsync(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var currentRoles = await _userManager.GetRolesAsync(currentUser);
            var currentRole = currentRoles.FirstOrDefault(); // or directly using currentRoles.FirstOrDefault() if only one role

            var policy = RoleUpdatePolicyFactory.CreatePolicy(currentRole);
            // Kiểm tra quyền đọc thông tin của người dùng khác
            if (!policy.CanReadOtherUsers(currentRole) && currentUser.Id != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to view this user's information.");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserDtoProfileResponse>(user);
            userDto.Role = roles.FirstOrDefault();
            return userDto;
        }

        public async Task<UserAccountDataDtoResponse> ResetPasswordAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");
            var defaultPassword = "123456789";
            // Bắt buộc phải có token hợp lệ để reset
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, defaultPassword);
            if (!result.Succeeded)
                throw new Exception("Password reset failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            var response = _mapper.Map<UserAccountDataDtoResponse>(user);
            response.Description = "Reset password successfully";
            // Dùng AutoMapper nếu bạn muốn
            return response;
        }

        public async Task<UserAccountDataDtoResponse> UpdatePasswordAsync(ChangePasswordRequestDto request, string userId)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null) throw new UnauthorizedAccessException("Not authenticated");

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
                throw new Exception("Password update failed");

            return _mapper.Map<UserAccountDataDtoResponse>(user);
        }

        public async Task<UserDtoProfileResponse> UpdateProfileAsync(UserProfileDtoRequest request, string userId)
        {

            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (currentUser == null) throw new UnauthorizedAccessException("User not authenticated");

            _mapper.Map(request, currentUser);
            var result = await _userManager.UpdateAsync(currentUser);
            if (!result.Succeeded)
                throw new Exception("Update profile failed");

            var roles = await _userManager.GetRolesAsync(currentUser);
            var dto = _mapper.Map<UserDtoProfileResponse>(currentUser);
            dto.Role = roles.FirstOrDefault();
            return dto;
        }

        public async Task<UserDtoResponse> UpdateRoleAsync(UserRoleDtoRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var currentRoles = await _userManager.GetRolesAsync(currentUser);
            var currentRole = currentRoles.FirstOrDefault(); // or directly using currentRoles.FirstOrDefault() if only one role

            var policy = RoleUpdatePolicyFactory.CreatePolicy(currentRole);
            if (!policy.CanUpdateToRole(request.Role))
            {
                throw new UnauthorizedAccessException("You are not authorized to update to this role.");
            }
            var oldRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, oldRoles);
            var result = await _userManager.AddToRoleAsync(user, request.Role);
            if (!result.Succeeded)
                throw new Exception("Failed to assign new role");


            var dto = _mapper.Map<UserDtoResponse>(user);
            dto.Role = request.Role;
            return dto;
        }
    }
}