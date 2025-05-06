using System.Linq.Expressions;
using Library_API_2._0.Application.Repositories;
using Library_API_2._0.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library_API_2._0.Infrastructure.Repository
{
    public class RecordRepository : GenericRepository<BorrowingRecord>, IRecordRepo
    {
        private readonly AppDbContext _context;
        public RecordRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<BorrowingRecord> GetIdAsync(Guid id)
        {
            return await _context.Records
                .Include(r => r.BorrowingRequest)
                .Include(r => r.User)
                .Include(r => r.PickUpAdmin)
                .Include(r => r.ReturnAdmin)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        public override async Task<IEnumerable<BorrowingRecord>> FindAsync(Expression<Func<BorrowingRecord, bool>> predicate)
        {
            return await _context.Records
                .Include(r => r.BorrowingRequest)
                .Include(r => r.User)
                .Include(r => r.PickUpAdmin)
                .Include(r => r.ReturnAdmin)
                .Where(predicate)
                .ToListAsync();
        }
    }
}