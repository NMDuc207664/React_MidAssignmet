using System.ComponentModel.DataAnnotations;

namespace Library_API_2._0.Domain.Entities
{
    public class Genre
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
    }
}
