using Library_API_2._0.Domain.Entities;

namespace Library_API_2._0.Application.Repositories
{
    public interface IBookRepo : IGenericRepo<Book>
    {
        Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(Guid authorId);
        Task<IEnumerable<Book>> GetBooksByGenreIdAsync(Guid genreId);
    }
}