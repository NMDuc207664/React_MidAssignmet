using Library_API_2._0.Domain.Entities;

namespace Library_API_2._0.Application.Repositories
{
    public interface IBorrowingRequestRepo : IGenericRepo<BorrowingRequest>
    {
        Task<IEnumerable<BorrowingRequest>> GetBorrowingRequestsByUserAsync(string userId);
        Task<IEnumerable<BorrowingRequest>> GetAllWithBookAsync();
        Task<int> GetMonthlyRequestCountAsync(string userId, DateTime currentDate);
        Task<List<BorrowingDetail>> GetBorrowingDetailsByRequestIdsAsync(List<Guid> requestIds);
    }
}