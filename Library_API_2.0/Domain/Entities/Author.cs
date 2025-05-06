
namespace Library_API_2._0.Domain.Entities
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}