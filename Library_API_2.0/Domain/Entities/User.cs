using Microsoft.AspNetCore.Identity;

namespace Library_API_2._0.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public ICollection<BorrowingRequest> BorrowingRequests { get; set; } = new List<BorrowingRequest>();
        public ICollection<BorrowingRecord> BorrowingRecords { get; set; } = new List<BorrowingRecord>();

    }
}