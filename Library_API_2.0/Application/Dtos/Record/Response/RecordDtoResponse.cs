using Library_API_2._0.Application.Dtos.Users.AdminRequest;
using Library_API_2._0.Domain.Enum;

namespace Library_API_2._0.Application.Dtos.Record.Response
{
    public class RecordDtoResponse
    {
        public Guid Id { get; set; }
        public Guid BorrowingRequestId { get; set; }
        public string UserId { get; set; }
        public bool HasPickedUp { get; set; }
        public DateTime? PickUpDate { get; set; }
        public string? PickUpAdminId { get; set; }
        public bool HasReturned { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? ReturnAdminId { get; set; }
        public ReturnStatus? ReturnStatus { get; set; }
    }
}