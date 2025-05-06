namespace Library_API_2._0.Domain.Entities
{
    public class BorrowingDetail
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid BorrowingRequestId { get; set; }
        public BorrowingRequest BorrowingRequest { get; set; }
    }
}