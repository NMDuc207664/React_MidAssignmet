using System.Security.Claims;
using AutoMapper;
using Library_API_2._0.Application.Dtos.Datetime;
using Library_API_2._0.Application.Dtos.Record.Response;
using Library_API_2._0.Application.Dtos.Users.AdminRequest;
using Library_API_2._0.Application.Interface;
using Library_API_2._0.Application.Repositories;
using Library_API_2._0.Domain.Entities;
using Library_API_2._0.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Library_API_2._0.Application.Service
{
    public class BorrowingRecordService : IBorrowingRecordService
    {
        private readonly IRecordRepo _recordRepository;
        private readonly IBorrowingRequestRepo _borrowingRequestRepository;
        private readonly IBookRepo _bookRepo;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _borrowingPeriodDays = 1;

        public BorrowingRecordService(
            IRecordRepo recordRepository,
            IBorrowingRequestRepo borrowingRequestRepository,
            IBookRepo bookRepo,
            UserManager<User> userManager,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _recordRepository = recordRepository;
            _borrowingRequestRepository = borrowingRequestRepository;
            _bookRepo = bookRepo;
            _userManager = userManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<RecordDtoResponse> CreateRecordAsync(Guid borrowingRequestId)
        {
            var borrowingRequest = await _borrowingRequestRepository.GetIdAsync(borrowingRequestId);
            if (borrowingRequest == null)
                throw new Exception("Borrowing request not found.");
            if (borrowingRequest.RequestStatus != RequestStatus.Approved)
                throw new Exception("Borrowing request must be approved to create a record.");

            var record = new BorrowingRecord
            {
                Id = Guid.NewGuid(),
                BorrowingRequestId = borrowingRequestId,
                BorrowingRequest = borrowingRequest,
                UserId = borrowingRequest.UserId,
                User = borrowingRequest.User,
                HasPickedUp = false,
                PickUpDate = null,
                PickUpAdminId = null,
                PickUpAdmin = null,
                HasReturned = false,
                ReturnDate = null,
                ReturnAdminId = null,
                ReturnAdmin = null,
                ReturnStatus = null,
            };

            await _recordRepository.AddAsync(record);
            await _recordRepository.SaveChangesAsync();
            return _mapper.Map<RecordDtoResponse>(record);
        }

        public async Task<RecordDtoResponse> MarkBookPickedUpAsync(Guid recordId, AdminRequestId adminId)
        {
            var record = await _recordRepository.GetIdAsync(recordId);
            if (record == null)
                throw new Exception("Borrowing record not found.");

            if (record.HasPickedUp)
                throw new Exception("Books have already been picked up.");

            var authenticationId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User is not authenticated.");

            var admin = await _userManager.FindByIdAsync(adminId.Id);
            if (admin == null)
                throw new Exception("Admin not found.");
            if (authenticationId != adminId.Id)
                throw new UnauthorizedAccessException("You are not authorized to pick up books.");

            record.HasPickedUp = true;
            record.PickUpDate = DateTime.Now;
            record.PickUpAdminId = authenticationId;
            record.PickUpAdmin = admin;

            await _recordRepository.UpdateAsync(record);
            await _recordRepository.SaveChangesAsync();
            return _mapper.Map<RecordDtoResponse>(record);
        }

        public async Task<RecordDtoResponse> MarkBookReturnedAsync(Guid recordId, AdminRequestId adminId)
        {
            var record = await _recordRepository.GetIdAsync(recordId);
            if (record == null)
                throw new Exception("Borrowing record not found.");

            if (record.HasReturned)
                throw new Exception("Books have already been returned.");

            if (!record.HasPickedUp)
                throw new Exception("Books must be picked up before returning.");

            var authenticationId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User is not authenticated.");

            var admin = await _userManager.FindByIdAsync(adminId.Id);
            if (admin == null)
                throw new Exception("Admin not found.");
            if (authenticationId != adminId.Id)
                throw new UnauthorizedAccessException("You are not authorized to return books.");

            record.HasReturned = true;
            record.ReturnDate = DateTime.Now;
            record.ReturnAdminId = authenticationId;
            record.ReturnAdmin = admin;

            // Trả sách về kho từ BorrowingDetails
            var borrowingRequest = await _borrowingRequestRepository.GetIdAsync(record.BorrowingRequestId);
            if (borrowingRequest?.BorrowingDetails.Any() == true)
            {
                var bookIds = borrowingRequest.BorrowingDetails.Select(bd => bd.BookId).ToList();
                var books = await _bookRepo.FindAsync(b => bookIds.Contains(b.Id));
                foreach (var book in books)
                {
                    book.StorageNumber += 1;
                    await _bookRepo.UpdateAsync(book);
                }
                await _bookRepo.SaveChangesAsync();
            }

            await _recordRepository.UpdateAsync(record);
            await _recordRepository.SaveChangesAsync();
            return _mapper.Map<RecordDtoResponse>(record);
        }

        public async Task<RecordDtoResponse> GetRecordByIdAsync(Guid id)
        {
            var record = await _recordRepository.GetIdAsync(id);
            if (record == null)
                throw new Exception("Borrowing record not found.");

            return _mapper.Map<RecordDtoResponse>(record);
        }


        public async Task<IEnumerable<RecordDtoResponse>> GetAllRecordsAsync()
        {
            var records = await _recordRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RecordDtoResponse>>(records);
        }

        public async Task<RecordDtoResponse> GetRecordByBorrowingRequestIdAsync(Guid borrowingRequestId)
        {
            var records = await _recordRepository.FindAsync(r => r.BorrowingRequestId == borrowingRequestId);
            var record = records.FirstOrDefault();
            if (record == null)
                throw new Exception("Borrowing record not found.");

            return _mapper.Map<RecordDtoResponse>(record);
        }

        public async Task DeleteRecordAsync(Guid id)
        {
            var record = await _recordRepository.GetIdAsync(id);
            if (record == null)
                throw new Exception("Borrowing record not found.");

            // Không cho phép xóa nếu HasPickedUp = true và HasReturned = false
            if (record.HasPickedUp && !record.HasReturned)
            {
                throw new InvalidOperationException("Cannot delete record when books have been picked up but not returned.");
            }

            // Nếu HasReturned = false, HasPickedUp = false và ReturnStatus != NotPickUp, trả sách về kho
            if (!record.HasReturned && !record.HasPickedUp && record.ReturnStatus != ReturnStatus.NotPickUp)
            {
                var borrowingRequest = await _borrowingRequestRepository.GetIdAsync(record.BorrowingRequestId);
                if (borrowingRequest?.BorrowingDetails.Any() == true)
                {
                    var bookIds = borrowingRequest.BorrowingDetails.Select(bd => bd.BookId).ToList();
                    var books = await _bookRepo.FindAsync(b => bookIds.Contains(b.Id));
                    foreach (var book in books)
                    {
                        book.StorageNumber += 1;
                        await _bookRepo.UpdateAsync(book);
                    }
                    await _bookRepo.SaveChangesAsync();
                }
            }

            // Xóa record
            await _recordRepository.DeleteAsync(id);
            await _recordRepository.SaveChangesAsync();
        }
        public async Task UpdateRecordStatusbyDayAsync(RequestDateTime currentTime)
        {
            var records = await _recordRepository.FindAsync(r =>
                r.ReturnStatus == null &&
                r.BorrowingRequest.RequestStatus == RequestStatus.Approved &&
                (
                    (!r.HasPickedUp && r.BorrowingRequest.ApprovedorDeniedDate.HasValue &&
                     r.BorrowingRequest.ApprovedorDeniedDate.Value <= currentTime.Date.AddMinutes(-_borrowingPeriodDays)) ||
                    (r.HasPickedUp && !r.HasReturned && r.PickUpDate.HasValue &&
                     r.PickUpDate.Value <= currentTime.Date.AddMinutes(-_borrowingPeriodDays))
                ));

            foreach (var record in records)
            {
                var request = await _borrowingRequestRepository.GetIdAsync(record.BorrowingRequestId);
                if (request == null || request.ApprovedorDeniedDate == null)
                {
                    Console.WriteLine($"Request {record.BorrowingRequestId} is null or ApprovedorDeniedDate is null");
                    continue;
                }

                var approvedDate = request.ApprovedorDeniedDate.Value;
                var dueDate = approvedDate.AddMinutes(_borrowingPeriodDays);
                Console.WriteLine($"Record {record.Id}: ApprovedDate = {approvedDate}, CurrentTime = {currentTime}, DueDate = {dueDate}, HasPickedUp = {record.HasPickedUp}");

                if (!record.HasPickedUp && currentTime.Date > dueDate)
                {
                    record.ReturnStatus = ReturnStatus.NotPickUp;
                    if (request.BorrowingDetails.Any())
                    {
                        var bookIds = request.BorrowingDetails.Select(bd => bd.BookId).ToList();
                        var books = await _bookRepo.FindAsync(b => bookIds.Contains(b.Id));
                        foreach (var book in books)
                        {
                            book.StorageNumber += 1;
                            await _bookRepo.UpdateAsync(book);
                        }
                        await _bookRepo.SaveChangesAsync();
                    }
                }
                else if (record.HasPickedUp && !record.HasReturned && record.PickUpDate.HasValue)
                {
                    var pickUpDueDate = record.PickUpDate.Value.AddMinutes(_borrowingPeriodDays);
                    if (currentTime.Date > pickUpDueDate)
                    {
                        record.ReturnStatus = ReturnStatus.Overdue;
                    }
                }

                if (record.ReturnStatus != null)
                {
                    await _recordRepository.UpdateAsync(record);
                    await _recordRepository.SaveChangesAsync();
                }
            }

            await _recordRepository.SaveChangesAsync();
        }
    }
}