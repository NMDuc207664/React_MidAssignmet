using System.ComponentModel.DataAnnotations;

namespace Library_API_2._0.Application.Dtos.Users.Request
{
    public class UserAuthRequestDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}