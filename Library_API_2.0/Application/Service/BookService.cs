using System.Linq.Expressions;
using AutoMapper;
using Library_API_2._0.Application.Dtos.Books;
using Library_API_2._0.Application.Dtos.Books.Response;
using Library_API_2._0.Application.Interface;
using Library_API_2._0.Application.Repositories;
using Library_API_2._0.Domain.Entities;

namespace Library_API_2._0.Application.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepo _bookRepo;
        private readonly IAuthorRepo _authorRepo;
        private readonly IGenreRepo _genreRepo;
        private readonly IMapper _mapper;
        public BookService(IBookRepo bookRepo, IAuthorRepo authorRepo, IGenreRepo genreRepo, IMapper mapper)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
            _genreRepo = genreRepo;
            _mapper = mapper;
        }

        public async Task<BooksDtoFullResponse> AddBookAsync(BooksCreateRequest request)
        {
            // Lấy danh sách ID tác giả và thể loại
            var authorIds = request.BookAuthors.Select(ba => ba.AuthorID).ToList();
            var genreIds = request.BookGenres.Select(bg => bg.GenreId).ToList();
            if (authorIds.Contains(Guid.Parse("B625C683-D11B-4B3D-B440-CFDD51C26E92")) && authorIds.Count > 1)
            {
                throw new Exception("It is not possible to assign another Author when the 'Updating' category is selected.");
            }
            if (genreIds.Contains(Guid.Parse("9BD8855A-634C-4248-8641-F7FEF2DC97C3")) && genreIds.Count > 1)
            {
                throw new Exception("It is not possible to assign another Category when the 'Updating' category is selected.");
            }
            // Kiểm tra có ID trùng lặp không
            if (authorIds.Count != authorIds.Distinct().Count())
            {
                throw new Exception("Duplicate author IDs found in request.");
            }

            if (genreIds.Count != genreIds.Distinct().Count())
            {
                throw new Exception("Duplicate genre IDs found in request.");
            }

            // Lấy danh sách tác giả và thể loại từ database
            var existingAuthors = await _authorRepo.FindAsync(a => authorIds.Contains(a.Id));
            var existingGenres = await _genreRepo.FindAsync(g => genreIds.Contains(g.Id));
            // Kiểm tra nếu có tác giả không tồn tại
            var missingAuthorIds = authorIds.Where(id => !existingAuthors.Any(a => a.Id == id)).ToList();
            if (missingAuthorIds.Any())
            {
                throw new Exception($"Author(s) with ID(s) {string.Join(", ", missingAuthorIds)} not found.");
            }

            // Kiểm tra nếu có thể loại không tồn tại
            var missingGenreIds = genreIds.Where(id => !existingGenres.Any(g => g.Id == id)).ToList();
            if (missingGenreIds.Any())
            {
                throw new Exception($"Genre(s) with ID(s) {string.Join(", ", missingGenreIds)} not found.");
            }

            // Tạo BookAuthors và BookGenres
            var bookAuthors = authorIds.Select(authorid => new BookAuthor
            {
                AuthorId = authorid,
            }).ToList();

            var bookGenres = genreIds.Select(genreid => new BookGenre
            {
                GenreId = genreid,
            }).ToList();

            // Tạo Book mới
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                StorageNumber = request.StorageNumber,
                BookAuthors = bookAuthors,
                BookGenres = bookGenres
            };

            await _bookRepo.AddAsync(book);
            await _bookRepo.SaveChangesAsync();

            return _mapper.Map<BooksDtoFullResponse>(book);
        }

        public async Task DeleteBookAsync(Guid id)
        {
            await _bookRepo.DeleteAsync(id);
            await _bookRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<BooksDtoFullResponse>> GetAllBooksAsync()
        {
            var books = await _bookRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<BooksDtoFullResponse>>(books);
        }

        public async Task<BooksDtoFullResponse> GetBookByIdAsync(Guid id)
        {
            var book = await _bookRepo.GetIdAsync(id);
            return _mapper.Map<BooksDtoFullResponse>(book);
        }

        public Task<IEnumerable<BooksDtoFullResponse>> GetBooksWithFiltersAsync(Expression<Func<Book, bool>> filter)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<BooksDtoFullResponse>> GetBooksByAuthorIdAsync(Guid authorId)
        {
            var books = await _bookRepo.GetBooksByAuthorIdAsync(authorId);
            return _mapper.Map<IEnumerable<BooksDtoFullResponse>>(books); // Map từ Book sang BookDto nếu cần
        }
        public async Task<IEnumerable<BooksDtoFullResponse>> GetBooksByGenreIdAsync(Guid genreId)
        {
            var books = await _bookRepo.GetBooksByGenreIdAsync(genreId);
            return _mapper.Map<IEnumerable<BooksDtoFullResponse>>(books); // Map từ Book sang BookDto nếu cần
        }

        public async Task<BooksDtoFullResponse> UpdateBookAsync(BooksCreateRequest request, Guid id)
        {
            // Kiểm tra book có tồn tại không
            var book = await _bookRepo.GetIdAsync(id);
            if (book == null)
            {
                throw new Exception($"Book with ID {id} not found.");
            }

            // Lấy danh sách ID tác giả và thể loại
            var authorIds = request.BookAuthors.Select(ba => ba.AuthorID).ToList();
            var genreIds = request.BookGenres.Select(bg => bg.GenreId).ToList();
            if (authorIds.Contains(Guid.Parse("B625C683-D11B-4B3D-B440-CFDD51C26E92")) && authorIds.Count > 1)
            {
                throw new Exception("It is not possible to assign another Author when the 'Updating' category is selected.");
            }
            if (genreIds.Contains(Guid.Parse("B625C683-D11B-4B3D-B440-CFDD51C26E92")) && genreIds.Count > 1)
            {
                throw new Exception("It is not possible to assign another Category when the 'Updating' category is selected.");
            }
            // Kiểm tra có ID trùng lặp không
            if (authorIds.Count != authorIds.Distinct().Count())
            {
                throw new Exception("Duplicate author IDs found in request.");
            }

            if (genreIds.Count != genreIds.Distinct().Count())
            {
                throw new Exception("Duplicate genre IDs found in request.");
            }

            // Lấy danh sách tác giả và thể loại từ database
            var existingAuthors = await _authorRepo.FindAsync(a => authorIds.Contains(a.Id));
            var existingGenres = await _genreRepo.FindAsync(g => genreIds.Contains(g.Id));

            // Kiểm tra nếu có tác giả không tồn tại
            var missingAuthorIds = authorIds.Where(id => !existingAuthors.Any(a => a.Id == id)).ToList();
            if (missingAuthorIds.Any())
            {
                throw new Exception($"Author(s) with ID(s) {string.Join(", ", missingAuthorIds)} not found.");
            }

            // Kiểm tra nếu có thể loại không tồn tại
            var missingGenreIds = genreIds.Where(id => !existingGenres.Any(g => g.Id == id)).ToList();
            if (missingGenreIds.Any())
            {
                throw new Exception($"Genre(s) with ID(s) {string.Join(", ", missingGenreIds)} not found.");
            }

            // Cập nhật các thông tin book
            book.Name = request.Name;
            book.Description = request.Description;
            book.StorageNumber = request.StorageNumber;

            // Cập nhật tác giả
            var bookAuthors = authorIds.Select(authorId => new BookAuthor
            {
                AuthorId = authorId
            }).ToList();

            // Cập nhật thể loại
            var bookGenres = genreIds.Select(genreId => new BookGenre
            {
                GenreId = genreId
            }).ToList();

            // Cập nhật BookAuthors và BookGenres
            book.BookAuthors = bookAuthors;
            book.BookGenres = bookGenres;

            await _bookRepo.UpdateAsync(book);
            await _bookRepo.SaveChangesAsync();

            return _mapper.Map<BooksDtoFullResponse>(book);
        }
    }
}