using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library_API_2._0.Application.Dtos.BorrowingDetails.Request
{
    public class BorrowingDetailCreateRequest
    {
        public Guid BookId { get; set; }
        public Guid RequestId { get; set; }
    }
}