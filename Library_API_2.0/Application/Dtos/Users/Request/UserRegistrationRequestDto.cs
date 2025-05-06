using System.ComponentModel.DataAnnotations;


namespace Library_API_2._0.Application.Dtos.Users.Request
{
    public class UserRegistrationRequestDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username must not contain special characters")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ComparePassword { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }
    }
}