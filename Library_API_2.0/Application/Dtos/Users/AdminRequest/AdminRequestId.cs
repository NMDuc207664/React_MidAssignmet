using System.ComponentModel.DataAnnotations;


namespace Library_API_2._0.Application.Dtos.Users.AdminRequest
{
    public class AdminRequestId
    {
        [Required]
        public string Id { get; set; }
    }
}