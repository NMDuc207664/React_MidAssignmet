using Library_API_2._0.Application.Dtos.BorrowingDetails.Response;
using Library_API_2._0.Domain.Enum;

namespace Library_API_2._0.Application.Dtos.BorrowingRequests.Response
{
    public class BorrowingRequestDtoFullResponse
    {
        public Guid Id { get; set; }
        public DateTime RequestedDate { get; set; }
        public RequestStatus RequestStatus { get; set; } = RequestStatus.Waiting;
        // Thông tin người dùng
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        //public int requestThisMonth { get; set; }
        public List<BorrowingDetailResponseDto> BorrowingDetails { get; set; } = new List<BorrowingDetailResponseDto>();
    }
}