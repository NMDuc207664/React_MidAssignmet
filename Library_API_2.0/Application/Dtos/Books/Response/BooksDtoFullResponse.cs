using Library_API_2._0.Application.Dtos.BookAuthor.Response;
using Library_API_2._0.Application.Dtos.BookGenre.Response;

namespace Library_API_2._0.Application.Dtos.Books.Response
{
    public class BooksDtoFullResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StorageNumber { get; set; }
        public ICollection<BookAuthorDtoResponse> BookAuthors { get; set; } = new List<BookAuthorDtoResponse>();
        public ICollection<BookGenreDtoResponse> BookGenres { get; set; } = new List<BookGenreDtoResponse>();
    }
}