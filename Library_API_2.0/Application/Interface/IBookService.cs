using System.Linq.Expressions;
using Library_API_2._0.Application.Dtos.Books;
using Library_API_2._0.Application.Dtos.Books.Response;
using Library_API_2._0.Domain.Entities;

namespace Library_API_2._0.Application.Interface
{
    public interface IBookService
    {
        Task<IEnumerable<BooksDtoFullResponse>> GetAllBooksAsync();
        Task<BooksDtoFullResponse> GetBookByIdAsync(Guid id);
        Task<BooksDtoFullResponse> AddBookAsync(BooksCreateRequest request);
        Task<BooksDtoFullResponse> UpdateBookAsync(BooksCreateRequest request, Guid id);
        Task DeleteBookAsync(Guid id);
        Task<IEnumerable<BooksDtoFullResponse>> GetBooksWithFiltersAsync(Expression<Func<Book, bool>> filter);
        Task<IEnumerable<BooksDtoFullResponse>> GetBooksByAuthorIdAsync(Guid authorId);
        Task<IEnumerable<BooksDtoFullResponse>> GetBooksByGenreIdAsync(Guid genreId);
    }
}