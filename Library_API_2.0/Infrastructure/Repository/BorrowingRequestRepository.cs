using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_API_2._0.Application.Repositories;
using Library_API_2._0.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library_API_2._0.Infrastructure.Repository
{
    public class BorrowingRequestRepository : GenericRepository<BorrowingRequest>, IBorrowingRequestRepo
    {
        private readonly AppDbContext _context;
        public BorrowingRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BorrowingRequest>> GetAllWithBookAsync()
        {
            return await _context.BorrowingRequests
                .Include(br => br.BorrowingDetails)
                .ThenInclude(bd => bd.Book)
                .ToListAsync();
        }
        public async Task<IEnumerable<BorrowingRequest>> GetBorrowingRequestsByUserAsync(string userId)
        {
            return await _context.BorrowingRequests
                .Where(r => r.UserId == userId)
                .Include(r => r.User)
                .Include(r => r.BorrowingDetails)
                    .ThenInclude(bd => bd.Book)
                .ToListAsync();
        }
        public override async Task<IEnumerable<BorrowingRequest>> GetAllAsync()
        {
            return await _context.BorrowingRequests
                .Include(br => br.User)
                .Include(b => b.BorrowingDetails)
                    .ThenInclude(ba => ba.Book)
                .ToListAsync();
        }

        public override async Task<BorrowingRequest> GetIdAsync(Guid id)
        {
            return await _context.BorrowingRequests
                    .Include(br => br.User) // để lấy thông tin user
                    .Include(br => br.BorrowingDetails) // để có danh sách sách mượn
                        .ThenInclude(d => d.Book) // nếu bạn muốn có luôn tên sách
                    .FirstOrDefaultAsync(br => br.Id == id);
        }

        public async Task<int> GetMonthlyRequestCountAsync(string userId, DateTime currentDate)
        {
            return await _context.BorrowingRequests
               .Where(r => r.UserId == userId && r.RequestedDate.Month == currentDate.Month && r.RequestedDate.Year == currentDate.Year)
                .CountAsync();
        }

        public async Task<List<BorrowingDetail>> GetBorrowingDetailsByRequestIdsAsync(List<Guid> requestIds)
        {
            if (requestIds == null || !requestIds.Any())
            {
                return new List<BorrowingDetail>();
            }

            return await _context.BorrowingDetails
                .Where(bd => requestIds.Contains(bd.BorrowingRequestId))
                .Include(bd => bd.Book) // Nạp thông tin Book để sử dụng tên sách nếu cần
                .ToListAsync();
        }
    }
}