using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_API_2._0.Application.Dtos.Authentication.Response
{
    public class AuthResponseDto
    {
        public bool IsAuthSucessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
    }
}