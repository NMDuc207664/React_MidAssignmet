using AutoMapper;
using Library_API_2._0.Application.Dtos.Authors;
using Library_API_2._0.Application.Dtos.Authors.Response;
using Library_API_2._0.Application.Dtos.BookAuthor.Request;
using Library_API_2._0.Application.Dtos.BookAuthor.Response;
using Library_API_2._0.Application.Dtos.BookGenre.Request;
using Library_API_2._0.Application.Dtos.BookGenre.Response;
using Library_API_2._0.Application.Dtos.Books;
using Library_API_2._0.Application.Dtos.Books.Response;
using Library_API_2._0.Application.Dtos.BorrowingDetails.Request;
using Library_API_2._0.Application.Dtos.BorrowingDetails.Response;
using Library_API_2._0.Application.Dtos.BorrowingRequests.Response;
using Library_API_2._0.Application.Dtos.Genres;
using Library_API_2._0.Application.Dtos.Genres.Response;
using Library_API_2._0.Application.Dtos.Record.Response;
using Library_API_2._0.Application.Dtos.Users.AdminRequest;
using Library_API_2._0.Application.Dtos.Users.Request;
using Library_API_2._0.Application.Dtos.Users.Response;
using Library_API_2._0.Domain.Entities;

namespace Library_API_2._0.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorDtoFullReponse>();
            CreateMap<Genre, GenreDtoFullResponse>();

            CreateMap<BookAuthorDtoRequest, BookAuthor>();
            CreateMap<BookGenreDtoRequest, BookGenre>();

            CreateMap<BorrowingRequest, BorrowingRequestDtoFullResponse>()
                   .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User != null ? src.User.FirstName + " " + src.User.LastName : null))
                   .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
                   .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User != null ? src.User.PhoneNumber : null))
                   .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User != null ? src.User.Address : null));
            CreateMap<BorrowingDetail, BorrowingDetailResponseDto>()
                    .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.Book.Name))
                    .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId));
            CreateMap<BorrowingDetail, BorrowingDetailDto>()
                    .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId));

            CreateMap<GenreDtoRequestbyName, Genre>();
            CreateMap<AuthorDtoRequestbyName, Author>();
            CreateMap<BooksCreateRequest, Book>();

            CreateMap<BorrowingDetailCreateRequest, BorrowingDetail>();
            CreateMap<BorrowingDetailUpdateRequest, BorrowingDetail>();
            CreateMap<User, AdminDto>()
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<User, AdminRequestId>();

            CreateMap<BorrowingRecord, RecordDtoResponse>();
            CreateMap<BorrowingRequest, BorrowingRequestResponstDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.requestedDate, opt => opt.MapFrom(src => src.RequestedDate))
                    .ForMember(dest => dest.requestStatus, opt => opt.MapFrom(src => src.RequestStatus))
                    .ForMember(dest => dest.Books, opt => opt.MapFrom(
                                src => src.BorrowingDetails.Select(bd => new BookDtoShortResponse
                                {
                                    Id = bd.Book.Id,
                                    Name = bd.Book.Name
                                }).ToList()
                            ));
            CreateMap<UserRegistrationRequestDto, User>();
            CreateMap<User, UserDtoResponse>()
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                    .ForMember(dest => dest.Role, opt => opt.Ignore()); // tránh lấy Role từ entity

            CreateMap<User, UserDtoProfileResponse>()
                    .ForMember(dest => dest.Role, opt => opt.Ignore()); // tương tự
            CreateMap<User, UserAccountDataDtoResponse>();
            CreateMap<UserProfileDtoRequest, User>();


            CreateMap<Book, BooksDtoFullResponse>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.BookAuthors, opt => opt.MapFrom(src => src.BookAuthors.Select(ba => new BookAuthorDtoResponse
                    {
                        AuthorID = ba.AuthorId,
                        AuthorName = ba.Author.Name
                    }).ToList()))
                    .ForMember(dest => dest.BookGenres, opt => opt.MapFrom(src => src.BookGenres.Select(bg => new BookGenreDtoResponse
                    {
                        GenreId = bg.GenreId,
                        GenreName = bg.Genre.Name
                    }).ToList()));

            CreateMap<BookAuthor, BookAuthorDtoResponse>()
                    .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.Name : null));

            CreateMap<BookGenre, BookGenreDtoResponse>()
                    .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre != null ? src.Genre.Name : null));

        }
    }
}