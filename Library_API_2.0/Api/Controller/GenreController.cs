using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_API_2._0.Application.Dtos.Genres;
using Library_API_2._0.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_API_2._0.Api.Controller
{
    [Route("api/genre")]
    [ApiController]

    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        [HttpGet]
        public async Task<IActionResult> GetGenresAsync()
        {
            var genres = await _genreService.GetGenresAsync();
            return Ok(genres);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetGenreByIdAsync(Guid id)
        {
            var genre = await _genreService.GetGenreByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateGenreAsync([FromBody] GenreDtoRequestbyName request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var genre = await _genreService.AddGenreAsync(request);
            if (genre == null)
            {
                return BadRequest("Failed to create genre."); // hoặc return Problem("...")
            }
            return Ok(genre);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateGenreAsync([FromBody] GenreDtoRequestbyName request, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var genre = await _genreService.UpdateGenreAsync(request, id);
            return Ok(genre);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGenreAsync(Guid id)
        {
            await _genreService.DeleteGenreAsync(id);
            return NoContent();
        }
    }
}