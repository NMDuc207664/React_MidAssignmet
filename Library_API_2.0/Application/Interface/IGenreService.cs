using Library_API_2._0.Application.Dtos.Genres;
using Library_API_2._0.Application.Dtos.Genres.Response;

namespace Library_API_2._0.Application.Interface
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDtoFullResponse>> GetGenresAsync();
        Task<GenreDtoFullResponse> GetGenreByIdAsync(Guid id);
        Task<GenreDtoFullResponse> AddGenreAsync(GenreDtoRequestbyName request);
        Task<GenreDtoFullResponse> UpdateGenreAsync(GenreDtoRequestbyName request, Guid Id);
        // Task<bool> UpdateBookAsync(Guid id);
        Task DeleteGenreAsync(Guid id);
    }
}