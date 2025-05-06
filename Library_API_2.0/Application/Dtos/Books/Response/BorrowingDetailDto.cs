using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library_API_2._0.Application.Dtos.Books.Response
{
    public class BorrowingDetailDto
    {
        [Required]
        public Guid BookId { get; set; }

        //[Required]
        //[Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        //public int Quantity { get; set; } = 1;
    }
}