using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_API_2._0.Application.Dtos.Authors;
using Library_API_2._0.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_API_2._0.Api.Controller
{
    [Route("api/author")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAuthorsAsync()
        {
            var authors = await _authorService.GetAuthorsAsync();
            return Ok(authors);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorByIdAsync(Guid id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAuthorAsync([FromBody] AuthorDtoRequestbyName request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var created = await _authorService.AddAuthorAsync(request);
            if (created == null)
            {
                return BadRequest("Failed to create author."); // hoặc return Problem("...")
            }
            return Ok(created);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAuthorAsync([FromBody] AuthorDtoRequestbyName request, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _authorService.UpdateAuthorAsync(request, id);
            return Ok(updated);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAuthorAsync(Guid id)
        {
            await _authorService.DeleteAuthorAsync(id);
            return NoContent();
        }
    }
}