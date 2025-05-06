
namespace Library_API_2._0.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StorageNumber { get; set; }
        public ICollection<BookAuthor> BookAuthors = new List<BookAuthor>();
        public ICollection<BookGenre> BookGenres = new List<BookGenre>();
        public ICollection<BorrowingDetail> BorrowingDetails = new List<BorrowingDetail>();
    }
}