using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Books;

namespace BookStoreApp.API.Repositories
{

	public interface IBooksRepository : IGenericRepository<Book>
	{
		public Task<List<BookReadOnlyDto>> GetAllBooksAsync();
		public Task<BookDetailsDto> GetBookAsync(int id);

	}
}
