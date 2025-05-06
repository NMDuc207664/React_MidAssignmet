using Library_API_2._0.Application.Dtos.Authentication.Response;
using Library_API_2._0.Application.Dtos.Users.Request;
using Library_API_2._0.Application.Dtos.Users.Response;

namespace Library_API_2._0.Application.Interface
{
    public interface IAccountService
    {
        Task<UserRegistrationResponseDto> RegisterAsync(UserRegistrationRequestDto request);
        Task<AuthResponseDto> AuthenticateAsync(UserAuthRequestDto request);
    }
}