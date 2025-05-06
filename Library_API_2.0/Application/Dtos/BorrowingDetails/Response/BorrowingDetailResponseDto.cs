using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_API_2._0.Application.Dtos.BorrowingDetails.Response
{
    public class BorrowingDetailResponseDto
    {
        public Guid BookId { get; set; }
        public string BookName { get; set; }
        //public int Quantity { get; set; }
    }
}