namespace Library_API_2._0.Application.Dtos.Users.Response
{
    public class UserRegistrationResponseDto
    {
        public bool IsSuccessfulRegistration { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}