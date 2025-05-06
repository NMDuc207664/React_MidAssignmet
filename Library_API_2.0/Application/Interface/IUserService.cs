using Library_API_2._0.Application.Dtos.Account.Request;
using Library_API_2._0.Application.Dtos.Users.Request;
using Library_API_2._0.Application.Dtos.Users.Response;

namespace Library_API_2._0.Application.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserDtoResponse>> GetAllUsersAsync();
        Task<UserDtoResponse> GetCurrentUserByAsync();
        Task<UserDtoProfileResponse> GetUserByIdAsync(string userId);
        Task<UserDtoProfileResponse> UpdateProfileAsync(UserProfileDtoRequest request, string userId);
        Task<UserDtoResponse> UpdateRoleAsync(UserRoleDtoRequest request, string userId);
        Task<UserAccountDataDtoResponse> UpdatePasswordAsync(ChangePasswordRequestDto request, string userId);
        Task<UserAccountDataDtoResponse> ResetPasswordAsync(string userId);
        Task DeleteUserAsync(string id);
    }
}