using System.Security.Claims;
using AutoMapper;
using Library_API_2._0.Application.Dtos.BorrowingDetails.Response;
using Library_API_2._0.Application.Dtos.BorrowingRequests.Request;
using Library_API_2._0.Application.Dtos.BorrowingRequests.Response;
using Library_API_2._0.Application.Interface;
using Library_API_2._0.Application.Repositories;
using Library_API_2._0.Domain.Entities;
using Library_API_2._0.Domain.Enum;
using Library_API_2._0.Domain.Factories;
using Microsoft.AspNetCore.Identity;

namespace Library_API_2._0.Application.Service
{
    public class BorrowingRequestService : IBorrowingRequestService
    {
        private readonly IBorrowingRequestRepo _borrowingRequestRepo;
        private readonly IBookRepo _bookRepo;
        private readonly IMapper _mapper;
        private readonly IBorrowingRecordService _borrowingRecordService;
        private readonly IRecordRepo _recordRepo;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BorrowingRequestService(IRecordRepo recordRepo, IBorrowingRequestRepo borrowingRequestRepo, IBookRepo bookRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, IBorrowingRecordService borrowingRecordService)
        {
            _borrowingRequestRepo = borrowingRequestRepo;
            _bookRepo = bookRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _borrowingRecordService = borrowingRecordService;
            _recordRepo = recordRepo;
        }

        public async Task<BorrowingRequestDtoFullResponse> CreateBorrowingRequestAsync(CreateBorrowingRequestDto requestDto)
        {
            var authenticationId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("Người dùng chưa được xác thực."));
            var targetUserId = string.IsNullOrWhiteSpace(requestDto.UserId)
                ? authenticationId.ToString()
                : requestDto.UserId;
            var targetUser = await _userManager.FindByIdAsync(targetUserId)
                ?? throw new Exception("Không tìm thấy người dùng.");
            var targetUserRoles = await _userManager.GetRolesAsync(targetUser);
            var targetRole = targetUserRoles.FirstOrDefault()
                ?? throw new Exception("Người dùng không có vai trò nào được gán.");

            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value
                ?? throw new UnauthorizedAccessException("Không tìm thấy vai trò.");
            if (requestDto.UserId != authenticationId.ToString() && role != "Admin")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền tạo yêu cầu cho người dùng khác.");
            }

            var policy = BorrowPolicyFactory.CreatePolicy(targetRole);
            var requestCountThisMonth = await _borrowingRequestRepo.GetMonthlyRequestCountAsync(targetUserId, DateTime.Now);

            var existingRequests = await _borrowingRequestRepo.GetBorrowingRequestsByUserAsync(targetUserId);
            if (!policy.CanBorrow(requestCountThisMonth))
            {
                throw new Exception("Bạn đã vượt quá số lượng yêu cầu tối đa trong tháng này.");
            }

            var bookIds = requestDto.BorrowingDetails.Select(bd => bd.BookId).ToList();
            var distinctBookIds = bookIds.Distinct().ToList();
            if (distinctBookIds.Count != bookIds.Count)
            {
                throw new ArgumentException("Bạn không thể mượn cùng một cuốn sách nhiều lần trong một yêu cầu.");
            }
            if (!policy.IsWithinBookLimit(bookIds.Count))
            {
                throw new Exception("Bạn không thể mượn quá 5 cuốn sách trong một yêu cầu.");
            }

            // Kiểm tra trùng lặp sách trong các request Waiting (chỉ áp dụng cho người dùng không phải admin)
            if (role != "Admin" || targetRole != "Admin")
            {
                var waitingRequests = existingRequests
                    .Where(r => r.RequestStatus == RequestStatus.Waiting)
                    .ToList();
                var waitingBookIds = waitingRequests
                    .SelectMany(r => r.BorrowingDetails.Select(d => d.BookId))
                    .ToList();
                var conflictingBookIds = bookIds.Intersect(waitingBookIds).ToList();
                if (conflictingBookIds.Any())
                {
                    throw new InvalidOperationException(
                        $"Bạn không thể mượn các sách đã có trong yêu cầu đang chờ: {string.Join(", ", conflictingBookIds)}.");
                }

                // Kiểm tra trùng lặp sách với BorrowingRecords có HasReturned = false hoặc ReturnStatus != NotPickedUp

                var activeRecords = await _recordRepo.FindAsync(r =>
                    r.HasReturned == false || r.ReturnStatus != ReturnStatus.NotPickUp);
                var activeRequestIds = activeRecords.Select(r => r.BorrowingRequestId).ToList();
                var activeBorrowingDetails = await _borrowingRequestRepo.GetBorrowingDetailsByRequestIdsAsync(activeRequestIds);
                var activeRecordBookIds = activeBorrowingDetails.Select(d => d.BookId).ToList();
                var conflictingRecordBookIds = bookIds.Intersect(activeRecordBookIds).ToList();
                if (conflictingRecordBookIds.Any())
                {
                    throw new InvalidOperationException(
                        $"Bạn không thể mượn các sách đang được mượn hoặc chưa được lấy: {string.Join(", ", conflictingRecordBookIds)}.");
                }

            }

            var existingBooks = await _bookRepo.FindAsync(b => bookIds.Contains(b.Id));
            foreach (var bookId in bookIds)
            {
                var book = existingBooks.FirstOrDefault(b => b.Id == bookId);
                if (book == null)
                {
                    throw new Exception($"Không tìm thấy sách với ID {bookId}.");
                }
                if (book.StorageNumber <= 0)
                {
                    throw new Exception($"Sách {book.Name} hiện đã hết hàng.");
                }
                book.StorageNumber -= 1;
                await _bookRepo.UpdateAsync(book);
            }
            await _bookRepo.SaveChangesAsync();

            var borrowingDetails = distinctBookIds.Select(bookId => new BorrowingDetail
            {
                BookId = bookId,
                Book = existingBooks.First(b => b.Id == bookId)
            }).ToList();

            var borrowingRequest = new BorrowingRequest
            {
                Id = Guid.NewGuid(),
                RequestedDate = DateTime.Now,
                RequestStatus = RequestStatus.Waiting,
                UserId = targetUserId,
                BorrowingDetails = borrowingDetails
            };

            var response = _mapper.Map<BorrowingRequestDtoFullResponse>(borrowingRequest);
            await _borrowingRequestRepo.AddAsync(borrowingRequest);
            await _borrowingRequestRepo.SaveChangesAsync();

            return response;
        }

        public async Task DeleteBorrowingRequestAsync(Guid id)
        {
            var authenticationId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User is not authenticated."));

            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value
                ?? throw new UnauthorizedAccessException("Role not found.");

            // Tìm yêu cầu mượn cần xóa
            var request = await _borrowingRequestRepo.GetIdAsync(id);
            if (request == null)
            {
                throw new InvalidOperationException("Borrowing request not found.");
            }

            // Tạo policy dựa trên role
            var policy = BorrowPolicyFactory.CreatePolicy(role);

            // Kiểm tra xem user có quyền xóa yêu cầu không
            if (role == "Admin" || (role != "Admin" && request.UserId == authenticationId.ToString())) // Admin có thể xóa mọi người, User chỉ có thể xóa chính mình
            {
                if (policy.CanDeleteRequest(id, request, DateTime.Now))
                {
                    // Nếu trạng thái là Waiting, trả sách về kho
                    if (request.RequestStatus == RequestStatus.Waiting)
                    {
                        var bookIds = request.BorrowingDetails.Select(d => d.BookId).ToList();
                        var books = await _bookRepo.FindAsync(b => bookIds.Contains(b.Id));
                        foreach (var book in books)
                        {
                            book.StorageNumber += 1;
                            await _bookRepo.UpdateAsync(book);
                        }
                        await _bookRepo.SaveChangesAsync();
                    }

                    // Xóa yêu cầu
                    await _borrowingRequestRepo.DeleteAsync(id);
                }
                else
                {
                    throw new InvalidOperationException("You cannot delete this request after 10 minutes.");
                }
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this request.");
            }
            await _borrowingRequestRepo.SaveChangesAsync();
        }


        public async Task<IEnumerable<BorrowingRequestDtoFullResponse>> GetAllBorrowingRequestsAsync()
        {
            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value
                ?? throw new UnauthorizedAccessException("Role not found.");
            if (role != "Admin")
            {
                throw new UnauthorizedAccessException("Only admins can view all borrowing requests.");
            }

            var requests = await _borrowingRequestRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<BorrowingRequestDtoFullResponse>>(requests);
        }

        public async Task<BorrowingRequestDtoFullResponse> GetBorrowingRequestByIdAsync(Guid id)
        {
            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value
              ?? throw new UnauthorizedAccessException("Role not found.");
            var authenticationId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User is not authenticated."));
            var request = await _borrowingRequestRepo.GetIdAsync(id)
            ?? throw new InvalidOperationException($"Borrowing request with ID {id} not found.");
            if (request.UserId != authenticationId.ToString() && role != "Admin")
            {
                throw new UnauthorizedAccessException("You are not authorized to view this borrowing request.");
            }
            var countThisMonth = await _borrowingRequestRepo.GetMonthlyRequestCountAsync(request.UserId, DateTime.Now);
            var response = _mapper.Map<BorrowingRequestDtoFullResponse>(request);
            //response.requestThisMonth = countThisMonth;
            return response;
        }

        public async Task<IEnumerable<BorrowingRequestDtoFullResponse>> GetBorrowingRequestByUserIdAsync(string id)//user xem được hết borrowrequest của mình, admin xem được của mọi người
        {
            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value
                ?? throw new UnauthorizedAccessException("Role not found.");
            var authenticationId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User is not authenticated."));
            if (id != authenticationId.ToString() && role != "Admin")
            {
                throw new UnauthorizedAccessException("You are not authorized to view borrowing requests of another user.");
            }
            var requests = await _borrowingRequestRepo.GetBorrowingRequestsByUserAsync(id);

            // var responses = _mapper.Map<IEnumerable<BorrowingRequestDtoFullResponse>>(requests);
            // responses.requestThisMonth = requestCountThisMonth;

            return _mapper.Map<IEnumerable<BorrowingRequestDtoFullResponse>>(requests);
        }

        public async Task<BorrowingRequestDtoFullResponse> UpdateBorrowingRequestStatusAsync(Guid id, BorrowingRequestUpdateRequest newStatus)//only admin
        {
            // Kiểm tra quyền admin
            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value
                ?? throw new UnauthorizedAccessException("Role not found.");
            if (role != "Admin")
            {
                throw new UnauthorizedAccessException("Only admins can update borrowing request status.");
            }

            // Tìm yêu cầu
            var request = await _borrowingRequestRepo.GetIdAsync(id)
                ?? throw new InvalidOperationException($"Borrowing request with ID {id} not found.");

            // Kiểm tra trạng thái hợp lệ
            if (!Enum.IsDefined(typeof(RequestStatus), newStatus.RequestStatus))
            {
                throw new ArgumentOutOfRangeException(nameof(newStatus.RequestStatus), "Invalid request status value.");
            }

            // Cập nhật trạng thái
            request.RequestStatus = newStatus.RequestStatus;

            if (newStatus.RequestStatus == RequestStatus.Approved)
            {
                request.ApprovedorDeniedDate = DateTime.Now;
                await _borrowingRequestRepo.UpdateAsync(request);
                await _borrowingRequestRepo.SaveChangesAsync();
                await _borrowingRecordService.CreateRecordAsync(id);
            }
            else if (newStatus.RequestStatus == RequestStatus.Rejected)
            {
                request.ApprovedorDeniedDate = DateTime.Now;
                var bookIds = request.BorrowingDetails.Select(d => d.BookId).ToList();
                var books = await _bookRepo.FindAsync(b => bookIds.Contains(b.Id));
                foreach (var book in books)
                {
                    book.StorageNumber += 1;
                    await _bookRepo.UpdateAsync(book);
                }
                await _bookRepo.SaveChangesAsync();
            }
            else
            {
                request.ApprovedorDeniedDate = null; // Xóa nếu không phải Approved hoặc Rejected
            }

            // Lưu thay đổi
            await _borrowingRequestRepo.UpdateAsync(request);
            await _borrowingRequestRepo.SaveChangesAsync();

            return _mapper.Map<BorrowingRequestDtoFullResponse>(request);
        }
    }
}