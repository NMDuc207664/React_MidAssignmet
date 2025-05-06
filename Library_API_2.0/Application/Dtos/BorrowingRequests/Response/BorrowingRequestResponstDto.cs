using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_API_2._0.Application.Dtos.Books.Response;
using Library_API_2._0.Domain.Enum;

namespace Library_API_2._0.Application.Dtos.BorrowingRequests.Response
{
    public class BorrowingRequestResponstDto
    {
        public Guid Id { get; set; }
        public DateTime requestedDate { get; set; }
        public RequestStatus requestStatus { get; set; } = RequestStatus.Waiting;
        public ICollection<BookDtoShortResponse> Books { get; set; } = new List<BookDtoShortResponse>();
    }
}