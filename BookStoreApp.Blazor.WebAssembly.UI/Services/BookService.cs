using AutoMapper;
using Blazored.LocalStorage;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services;

public class BookService : BaseHttpService, IBookService
{
	private readonly IClient _client;
	private readonly IMapper _mapper;

	public BookService(IClient client, ILocalStorageService localStorage, IMapper mapper) : base(client, localStorage)
	{
		_client = client;
		_mapper = mapper;
	}

	public async Task<Response<List<BookReadOnlyDto>>> Get()
	{
		Response<List<BookReadOnlyDto>> response;

		try
		{
			await GetBearerToken();
			var data = await _client.BooksAllAsync();
			response = new Response<List<BookReadOnlyDto>>
			{
				Data = data.ToList(),
				Success = true
			};
		}
		catch (ApiException exception)
		{
			response = ConvertApiExceptions<List<BookReadOnlyDto>>(exception);
		}

		return response;
	}
	public async Task<Response<BookDetailsDto>> Get(int Id)
	{
		Response<BookDetailsDto> response;

		try
		{
			await GetBearerToken();
			var data = await _client.BooksGETAsync(Id);
			response = new Response<BookDetailsDto>
			{
				Data = data,
				Success = true
			};
		}
		catch (ApiException exception)
		{
			response = ConvertApiExceptions<BookDetailsDto>(exception);
		}

		return response;
	}
	public async Task<Response<BookUpdateDto>> GetForUpdate(int Id)
	{
		Response<BookUpdateDto> response;

		try
		{
			await GetBearerToken();
			var data = await _client.BooksGETAsync(Id);
			response = new Response<BookUpdateDto>
			{
				Data = _mapper.Map<BookUpdateDto>(data),
				Success = true
			};
		}
		catch (ApiException exception)
		{
			response = ConvertApiExceptions<BookUpdateDto>(exception);
		}

		return response;
	}

	public async Task<Response<int>> Create(BookCreateDto Book)
	{
		Response<int> response = new Response<int>();
		try
		{
			await GetBearerToken();
			await _client.BooksPOSTAsync(Book);
		}
		catch (ApiException exception)
		{
			response = ConvertApiExceptions<int>(exception);
		}

		return response;
	}
	public async Task<Response<int>> Edit(int id, BookUpdateDto Book)
	{
		Response<int> response = new Response<int>();
		try
		{
			await GetBearerToken();
			await _client.BooksPUTAsync(id, Book);
		}
		catch (ApiException exception)
		{
			response = ConvertApiExceptions<int>(exception);
		}

		return response;
	}

	public async Task<Response<int>> Delete(int id)
	{
		Response<int> response = new Response<int>();
		try
		{
			await GetBearerToken();
			await _client.BooksDELETEAsync(id);
		}
		catch (ApiException exception)
		{
			response = ConvertApiExceptions<int>(exception);
		}

		return response;
	}
}