using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_API_2._0.Application.Repositories;
using Library_API_2._0.Domain.Entities;

namespace Library_API_2._0.Infrastructure.Repository
{
    public class BorrowingDetailRepository : GenericRepository<BorrowingDetail>, IBorrowingDetailRepo
    {
        private readonly AppDbContext _context;
        public BorrowingDetailRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}