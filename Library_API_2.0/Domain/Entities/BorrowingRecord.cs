using Library_API_2._0.Domain.Enum;

namespace Library_API_2._0.Domain.Entities
{
    public class BorrowingRecord
    {
        public Guid Id { get; set; }
        public Guid BorrowingRequestId { get; set; }
        public BorrowingRequest BorrowingRequest { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool HasPickedUp { get; set; } = false;
        public DateTime? PickUpDate { get; set; }
        public string? PickUpAdminId { get; set; }
        public User? PickUpAdmin { get; set; }
        public bool HasReturned { get; set; } = false;
        public DateTime? ReturnDate { get; set; }
        public string? ReturnAdminId { get; set; }
        public User? ReturnAdmin { get; set; }
        public ReturnStatus? ReturnStatus { get; set; }
    }
}