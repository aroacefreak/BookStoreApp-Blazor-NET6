using BookStoreApp.Blazor.Server.UI.Models;
using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services;

public interface IBookService
{
	Task<Response<BookReadOnlyDtoVirtualiseResponse>> Get(QueryParameters queryParams);
	Task<Response<List<BookReadOnlyDto>>> Get();
	Task<Response<BookDetailsDto>> Get(int Id);
	Task<Response<BookUpdateDto>> GetForUpdate(int Id);
	Task<Response<int>> Create(BookCreateDto Book);
	Task<Response<int>> Edit(int id, BookUpdateDto Book);
	Task<Response<int>> Delete(int id);
}