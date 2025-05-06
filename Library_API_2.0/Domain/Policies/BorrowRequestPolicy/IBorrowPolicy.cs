using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_API_2._0.Domain.Entities;

namespace Library_API_2._0.Domain.Policies.BorrowRequestPolicy
{
    public interface IBorrowPolicy
    {
        bool CanBorrow(int requestCount);
        bool IsWithinBookLimit(int totalBooks);
        bool CanDeleteRequest(Guid requestid, BorrowingRequest request, DateTime currentDate);
    }
}