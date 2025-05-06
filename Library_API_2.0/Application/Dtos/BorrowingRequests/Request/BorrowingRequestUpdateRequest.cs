using System.ComponentModel.DataAnnotations;
using Library_API_2._0.Domain.Enum;

namespace Library_API_2._0.Application.Dtos.BorrowingRequests.Request
{
    public class BorrowingRequestUpdateRequest
    {
        [Required]
        public RequestStatus RequestStatus { get; set; }
    }
}