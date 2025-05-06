using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_API_2._0.Application.Repositories;
using Library_API_2._0.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library_API_2._0.Infrastructure.Repository
{
    public class BookRepository : GenericRepository<Book>, IBookRepo
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(Guid authorId)
        {
            return await _context.Books
                                 .Where(b => b.BookAuthors.Any(ba => ba.AuthorId == authorId)) // Lọc sách theo AuthorId thông qua bảng trung gian BookAuthor
                                  .Include(b => b.BookAuthors)
                                        .ThenInclude(ba => ba.Author)
                                .Include(b => b.BookGenres)
                                        .ThenInclude(bg => bg.Genre)
                                 .ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetBooksByGenreIdAsync(Guid genreId)
        {
            return await _context.Books
                                 .Where(b => b.BookGenres.Any(bg => bg.GenreId == genreId)) // Lọc sách theo GenreId thông qua bảng trung gian BookGenre
                                 .Include(b => b.BookAuthors)
                                        .ThenInclude(ba => ba.Author)
                                .Include(b => b.BookGenres)
                                        .ThenInclude(bg => bg.Genre)
                                 .ToListAsync();
        }

        public override async Task<Book> GetIdAsync(Guid id)
        {
            return await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

    }
}