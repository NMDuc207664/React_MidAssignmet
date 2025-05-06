using Library_API_2._0.Application.Dtos.Datetime;
using Library_API_2._0.Application.Dtos.Record.Response;
using Library_API_2._0.Application.Dtos.Users.AdminRequest;

namespace Library_API_2._0.Application.Interface
{
    public interface IBorrowingRecordService
    {
        Task<IEnumerable<RecordDtoResponse>> GetAllRecordsAsync();
        Task<RecordDtoResponse> CreateRecordAsync(Guid borrowingRequestId);
        Task<RecordDtoResponse> MarkBookPickedUpAsync(Guid recordId, AdminRequestId adminId);
        Task<RecordDtoResponse> MarkBookReturnedAsync(Guid recordId, AdminRequestId adminId);
        Task<RecordDtoResponse> GetRecordByIdAsync(Guid id);
        Task DeleteRecordAsync(Guid id);
        Task<RecordDtoResponse> GetRecordByBorrowingRequestIdAsync(Guid borrowingRequestId);
        Task UpdateRecordStatusbyDayAsync(RequestDateTime date);
    }
}