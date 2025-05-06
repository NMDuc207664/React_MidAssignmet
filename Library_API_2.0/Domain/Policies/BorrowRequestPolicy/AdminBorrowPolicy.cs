using Library_API_2._0.Domain.Entities;

namespace Library_API_2._0.Domain.Policies.BorrowRequestPolicy
{
    public class AdminBorrowPolicy : IBorrowPolicy
    {
        public bool CanBorrow(int requestCountThisMonth)
        {
            return true; // Admin không giới hạn yêu cầu
        }

        public bool CanDeleteRequest(Guid requestId, BorrowingRequest request, DateTime currentDate)
        {
            return true;
        }

        public bool IsWithinBookLimit(int totalBooks)
        {
            return true; // Admin không giới hạn số lượng sách
        }

    }
}