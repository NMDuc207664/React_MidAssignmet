using System.ComponentModel.DataAnnotations;
using Library_API_2._0.Application.Dtos.Books.Response;

namespace Library_API_2._0.Application.Dtos.BorrowingRequests.Request
{
    public class CreateBorrowingRequestDto
    {
        public string? UserId { get; set; }
        [Required]
        public List<BorrowingDetailDto> BorrowingDetails { get; set; } = new List<BorrowingDetailDto>();
    }
}