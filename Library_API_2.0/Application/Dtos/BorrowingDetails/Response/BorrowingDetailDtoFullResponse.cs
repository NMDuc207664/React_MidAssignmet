using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_API_2._0.Domain.Entities;

namespace Library_API_2._0.Application.Dtos.BorrowingDetails.Response
{
    public class BorrowingDetailDtoFullResponse
    {
        public Guid BookID { get; set; }
        public Book Book { get; set; }
        public Guid BorrowingRequestID { get; set; }
        public int Quantity { get; set; }
    }
}