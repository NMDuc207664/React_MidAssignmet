using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Library_API_2._0.Application.Dtos.BookAuthor.Request;
using Library_API_2._0.Application.Dtos.BookGenre.Request;

namespace Library_API_2._0.Application.Dtos.Books
{
    public class BooksCreateRequest
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public int StorageNumber { get; set; }
        public List<BookAuthorDtoRequest> BookAuthors { get; set; } = new List<BookAuthorDtoRequest>();
        public List<BookGenreDtoRequest> BookGenres { get; set; } = new List<BookGenreDtoRequest>();
    }
}