using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_API_2._0.Application.Dtos.Books;
using Library_API_2._0.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_API_2._0.Api.Controller
{
    [Route("/api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _booksService;
        public BookController(IBookService booksService)
        {
            _booksService = booksService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            var books = await _booksService.GetAllBooksAsync();
            return Ok(books);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookByIdAsync(Guid id)
        {
            var book = await _booksService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateBookAsync([FromBody] BooksCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var created = await _booksService.AddBookAsync(request);
            // return CreatedAtAction(nameof(GetBookByIdAsync), new { id = created.Id }, created);
            return Ok(created);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateBookAsync([FromBody] BooksCreateRequest request, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _booksService.UpdateBookAsync(request, id);
            return Ok(updated);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteBookAsync(Guid id)
        {
            await _booksService.DeleteBookAsync(id);
            return NoContent();
        }
        [HttpGet("by-author/{authorId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBooksByAuthorIdAsync(Guid authorId)
        {
            var books = await _booksService.GetBooksByAuthorIdAsync(authorId);
            return Ok(books);
        }
        [HttpGet("by-genre/{genreId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBooksByGenreIdAsync(Guid genreId)
        {
            var books = await _booksService.GetBooksByGenreIdAsync(genreId);
            return Ok(books);
        }
    }
}
