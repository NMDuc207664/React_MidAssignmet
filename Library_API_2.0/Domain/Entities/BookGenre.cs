namespace Library_API_2._0.Domain.Entities
{
    public class BookGenre
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}