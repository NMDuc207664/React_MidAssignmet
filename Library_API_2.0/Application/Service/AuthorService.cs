using AutoMapper;
using Library_API_2._0.Application.Dtos.Authors;
using Library_API_2._0.Application.Dtos.Authors.Response;
using Library_API_2._0.Application.Interface;
using Library_API_2._0.Application.Repositories;
using Library_API_2._0.Domain.Entities;

namespace Library_API_2._0.Application.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepo _authorRepo;
        private readonly IBookRepo _bookRepo;
        private readonly IMapper _mapper;
        public AuthorService(IAuthorRepo authorRepo, IMapper mapper, IBookRepo bookRepo)
        {
            _authorRepo = authorRepo;
            _mapper = mapper;
            _bookRepo = bookRepo;
        }
        public async Task<AuthorDtoFullReponse> AddAuthorAsync(AuthorDtoRequestbyName request)
        {
            var author = _mapper.Map<Author>(request);
            author.Id = Guid.NewGuid();
            await _authorRepo.AddAsync(author);
            await _authorRepo.SaveChangesAsync();
            return _mapper.Map<AuthorDtoFullReponse>(author);
        }

        public async Task DeleteAuthorAsync(Guid id)
        {
            var author = await _authorRepo.GetIdAsync(id);
            if (author == null)
            {
                throw new Exception($"Author with ID {id} not found.");
            }
            var booksWithAuthor = await _bookRepo.GetBooksByAuthorIdAsync(id);
            foreach (var book in booksWithAuthor)
            {
                // Cập nhật các Book để liên kết với Author mặc định
                var defaultAuthorId = Guid.Parse("B625C683-D11B-4B3D-B440-CFDD51C26E92");

                // Xóa tất cả Author hiện tại liên kết với Book (trừ author bị xóa)
                book.BookAuthors = book.BookAuthors.Where(ba => ba.AuthorId != id).ToList();

                // Nếu không còn author nào liên kết với Book, thêm Author mặc định
                if (!book.BookAuthors.Any())
                {
                    book.BookAuthors.Add(new BookAuthor { BookId = book.Id, AuthorId = defaultAuthorId });
                }
            }
            await _authorRepo.DeleteAsync(id);
            await _authorRepo.SaveChangesAsync();
        }

        public async Task<AuthorDtoFullReponse> GetAuthorByIdAsync(Guid id)
        {
            var author = await _authorRepo.GetIdAsync(id);
            if (author == null)
            {
                throw new Exception($"Author with ID {id} not found.");
            }
            return _mapper.Map<AuthorDtoFullReponse>(author);
        }

        public async Task<AuthorDtoFullReponse> UpdateAuthorAsync(AuthorDtoRequestbyName request, Guid id)
        {
            var author = await _authorRepo.GetIdAsync(id);
            _mapper.Map(request, author);
            await _authorRepo.UpdateAsync(author);
            await _authorRepo.SaveChangesAsync();
            return _mapper.Map<AuthorDtoFullReponse>(author);
        }
        public async Task<IEnumerable<AuthorDtoFullReponse>> GetAuthorsAsync()
        {
            var authors = await _authorRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<AuthorDtoFullReponse>>(authors);
        }
    }
}