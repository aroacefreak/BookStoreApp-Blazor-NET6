using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Models.Books;
using BookStoreApp.API.Models.User;

namespace BookStoreApp.API.Configurations
{
	public class MapperConfig: Profile
	{
		public MapperConfig()
		{
			//Authors
			CreateMap<AuthorCreateDto, Author>().ReverseMap();
			CreateMap<AuthorUpdateDto, Author>().ReverseMap();
			CreateMap<AuthorDetailsDto, Author>().ReverseMap();
			CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();
			CreateMap<BookReadOnlyDto, Book>().ReverseMap();

			//Books
			CreateMap<Book, BookCreateDto>().ReverseMap();
			CreateMap<Book, BookUpdateDto>().ReverseMap();
			CreateMap<Book, BookReadOnlyDto>()
				.ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
				.ReverseMap();
			CreateMap<Book, BookDetailsDto>()
				.ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
				.ReverseMap();

			//User
			CreateMap<UserDto, ApiUser>().ReverseMap();
		}
	}
}
