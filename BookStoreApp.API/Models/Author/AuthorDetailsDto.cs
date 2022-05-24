using BookStoreApp.API.Models.Books;

namespace BookStoreApp.API.Models.Author
{
	public class AuthorDetailsDto: AuthorReadOnlyDto
	{
		public List<BookReadOnlyDto> Books { get; set; }
	}
}
