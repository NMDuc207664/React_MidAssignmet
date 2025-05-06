using System.ComponentModel.DataAnnotations;

namespace Library_API_2._0.Application.Dtos.Users.Request
{
    public class UserRoleDtoRequest
    {
        [Required]
        public string Role { get; set; }
    }
}