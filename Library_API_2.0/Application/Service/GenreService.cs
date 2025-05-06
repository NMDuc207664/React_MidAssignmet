using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library_API_2._0.Application.Dtos.Genres;
using Library_API_2._0.Application.Dtos.Genres.Response;
using Library_API_2._0.Application.Interface;
using Library_API_2._0.Application.Repositories;
using Library_API_2._0.Domain.Entities;

namespace Library_API_2._0.Application.Service
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepo _genreRepo;
        private readonly IMapper _mapper;
        private readonly IBookRepo _bookRepo;
        public GenreService(IGenreRepo genreRepo, IMapper mapper, IBookRepo bookRepo)
        {
            _genreRepo = genreRepo;
            _mapper = mapper;
            _bookRepo = bookRepo;
        }
        public async Task<GenreDtoFullResponse> AddGenreAsync(GenreDtoRequestbyName request)
        {
            var genre = _mapper.Map<Genre>(request);
            genre.Id = Guid.NewGuid();
            await _genreRepo.AddAsync(genre);
            await _genreRepo.SaveChangesAsync();
            return _mapper.Map<GenreDtoFullResponse>(genre);
        }

        public async Task DeleteGenreAsync(Guid id)
        {
            var genre = await _genreRepo.GetIdAsync(id);
            if (genre == null)
            {
                throw new Exception($"Author with ID {id} not found.");
            }
            var booksWithGenre = await _bookRepo.GetBooksByGenreIdAsync(id);
            foreach (var book in booksWithGenre)
            {
                var defaultAuthorId = Guid.Parse("9BD8855A-634C-4248-8641-F7FEF2DC97C3");
                book.BookGenres = book.BookGenres.Where(ba => ba.GenreId != id).ToList();
                if (!book.BookGenres.Any())
                {
                    book.BookGenres.Add(new BookGenre { BookId = book.Id, GenreId = defaultAuthorId });
                }
            }
            await _genreRepo.DeleteAsync(id);
            await _genreRepo.SaveChangesAsync();
        }

        public async Task<GenreDtoFullResponse> GetGenreByIdAsync(Guid id)
        {
            var genre = await _genreRepo.GetIdAsync(id);
            await _genreRepo.SaveChangesAsync();
            return _mapper.Map<GenreDtoFullResponse>(genre);
        }

        public async Task<IEnumerable<GenreDtoFullResponse>> GetGenresAsync()
        {
            await _genreRepo.GetAllAsync();
            await _genreRepo.SaveChangesAsync();
            return _mapper.Map<IEnumerable<GenreDtoFullResponse>>(await _genreRepo.GetAllAsync());
        }

        public async Task<GenreDtoFullResponse> UpdateGenreAsync(GenreDtoRequestbyName request, Guid Id)
        {
            var genre = await _genreRepo.GetIdAsync(Id);
            _mapper.Map(request, genre);
            await _genreRepo.UpdateAsync(genre);
            await _genreRepo.SaveChangesAsync();
            return _mapper.Map<GenreDtoFullResponse>(genre);
        }
    }
}