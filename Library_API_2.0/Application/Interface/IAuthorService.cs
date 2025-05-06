using Library_API_2._0.Application.Dtos.Authors;
using Library_API_2._0.Application.Dtos.Authors.Response;

namespace Library_API_2._0.Application.Interface
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDtoFullReponse>> GetAuthorsAsync();
        Task<AuthorDtoFullReponse> GetAuthorByIdAsync(Guid id);
        Task<AuthorDtoFullReponse> AddAuthorAsync(AuthorDtoRequestbyName request);
        Task<AuthorDtoFullReponse> UpdateAuthorAsync(AuthorDtoRequestbyName request, Guid id);
        Task DeleteAuthorAsync(Guid id);
    }
}