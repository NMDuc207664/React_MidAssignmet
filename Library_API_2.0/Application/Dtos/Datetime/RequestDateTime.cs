using System.ComponentModel.DataAnnotations;
namespace Library_API_2._0.Application.Dtos.Datetime
{
    public class RequestDateTime
    {
        [Required]
        public DateTime Date { get; set; }
    }
}