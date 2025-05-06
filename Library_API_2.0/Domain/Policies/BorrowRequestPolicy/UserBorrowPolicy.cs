using Library_API_2._0.Domain.Entities;

namespace Library_API_2._0.Domain.Policies.BorrowRequestPolicy
{
    public class UserBorrowPolicy : IBorrowPolicy
    {
        public bool CanBorrow(int requestCountThisMonth)
        {
            return requestCountThisMonth < 3;
        }

        public bool CanDeleteRequest(Guid requestId, BorrowingRequest request, DateTime currentDate)
        {
            var timeDifference = currentDate - request.RequestedDate;
            if (timeDifference.TotalMinutes <= 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsWithinBookLimit(int totalBooks)
        {
            return totalBooks <= 5;
        }
    }
}