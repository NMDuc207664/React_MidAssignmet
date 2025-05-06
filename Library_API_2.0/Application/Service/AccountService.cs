using AutoMapper;
using Library_API_2._0.Application.Dtos.Authentication.Response;
using Library_API_2._0.Application.Dtos.Users.Request;
using Library_API_2._0.Application.Dtos.Users.Response;
using Library_API_2._0.Application.Interface;
using Library_API_2._0.Domain.Entities;
using Library_API_2._0.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Library_API_2._0.Application.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtHandler _jwtHandler;
        private readonly IMapper _mapper;
        public AccountService(UserManager<User> userManager, JwtHandler jwtHandler, IMapper mapper)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
        }
        public async Task<UserRegistrationResponseDto> RegisterAsync(UserRegistrationRequestDto request)
        {
            var user = _mapper.Map<User>(request);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new UserRegistrationResponseDto
                {
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            await _userManager.AddToRoleAsync(user, "User");
            return new UserRegistrationResponseDto();
        }

        public async Task<AuthResponseDto> AuthenticateAsync(UserAuthRequestDto request)
        {
            User user = request.Username.Contains("@")
                ? await _userManager.FindByEmailAsync(request.Username)
                : await _userManager.FindByNameAsync(request.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new AuthResponseDto { ErrorMessage = "Invalid authentication" };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtHandler.CreateToken(user, roles);

            return new AuthResponseDto
            {
                IsAuthSucessful = true,
                Token = token
            };
        }
    }
}