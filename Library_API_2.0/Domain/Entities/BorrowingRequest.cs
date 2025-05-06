using Library_API_2._0.Domain.Enum;

namespace Library_API_2._0.Domain.Entities
{
    public class BorrowingRequest
    {
        public Guid Id { get; set; }
        public DateTime RequestedDate { get; set; }
        public RequestStatus RequestStatus { get; set; } = RequestStatus.Waiting;
        public DateTime? ApprovedorDeniedDate { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<BorrowingDetail> BorrowingDetails { get; set; } = new List<BorrowingDetail>();


    }
}