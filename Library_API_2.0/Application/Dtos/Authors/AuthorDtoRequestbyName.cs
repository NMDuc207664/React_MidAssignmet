using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library_API_2._0.Application.Dtos.Authors
{
    public class AuthorDtoRequestbyName
    {
        [Required]
        public string Name { get; set; }
    }
}