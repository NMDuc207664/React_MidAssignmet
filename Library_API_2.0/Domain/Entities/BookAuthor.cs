namespace Library_API_2._0.Domain.Entities
{
    public class BookAuthor
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
    }
}