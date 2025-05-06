using Library_API_2._0.Application.Dtos.BorrowingRequests.Request;
using Library_API_2._0.Application.Dtos.BorrowingRequests.Response;

namespace Library_API_2._0.Application.Interface
{
    public interface IBorrowingRequestService
    {
        Task<BorrowingRequestDtoFullResponse> CreateBorrowingRequestAsync(CreateBorrowingRequestDto requestDto);
        Task<IEnumerable<BorrowingRequestDtoFullResponse>> GetBorrowingRequestByUserIdAsync(string id);
        Task<BorrowingRequestDtoFullResponse> GetBorrowingRequestByIdAsync(Guid id);
        Task<IEnumerable<BorrowingRequestDtoFullResponse>> GetAllBorrowingRequestsAsync();
        Task<BorrowingRequestDtoFullResponse> UpdateBorrowingRequestStatusAsync(Guid id, BorrowingRequestUpdateRequest newStatus);
        Task DeleteBorrowingRequestAsync(Guid id);
    }
}