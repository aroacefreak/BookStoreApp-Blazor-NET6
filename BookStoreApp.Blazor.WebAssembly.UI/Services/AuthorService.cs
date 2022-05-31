using AutoMapper;
using Blazored.LocalStorage;
using BookStoreApp.Blazor.WebAssembly.UI.Models;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services
{
	public class AuthorService : BaseHttpService, IAuthorService
	{
		private readonly IClient _client;
		private readonly IMapper _mapper;

		public AuthorService(IClient client, ILocalStorageService localStorage, IMapper mapper) : base(client, localStorage)
		{
			_client = client;
			_mapper = mapper;
		}

		public async Task<Response<AuthorReadOnlyDtoVirtualiseResponse>> Get(QueryParameters queryParams)
		{
			Response<AuthorReadOnlyDtoVirtualiseResponse> response;

			try
			{
				await GetBearerToken();
				var data = await _client.AuthorsGETAsync(queryParams.StartIndex, queryParams.PageSize);
				response = new Response<AuthorReadOnlyDtoVirtualiseResponse>
				{
					Data = data,
					Success = true
				};
			}
			catch (ApiException exception)
			{
				response = ConvertApiExceptions<AuthorReadOnlyDtoVirtualiseResponse>(exception);
			}

			return response;
		}
		public async Task<Response<AuthorDetailsDto>> Get(int Id)
		{
			Response<AuthorDetailsDto> response;

			try
			{
				await GetBearerToken();
				var data = await _client.AuthorsGET2Async(Id);
				response = new Response<AuthorDetailsDto>
				{
					Data = data,
					Success = true
				};
			}
			catch (ApiException exception)
			{
				response = ConvertApiExceptions<AuthorDetailsDto>(exception);
			}

			return response;
		}
		public async Task<Response<AuthorUpdateDto>> GetForUpdate(int Id)
		{
			Response<AuthorUpdateDto> response;

			try
			{
				await GetBearerToken();
				var data = await _client.AuthorsGET2Async(Id);
				response = new Response<AuthorUpdateDto>
				{
					Data = _mapper.Map<AuthorUpdateDto>(data),
					Success = true
				};
			}
			catch (ApiException exception)
			{
				response = ConvertApiExceptions<AuthorUpdateDto>(exception);
			}

			return response;
		}

		public async Task<Response<int>> Create(AuthorCreateDto author)
		{
			Response<int> response = new Response<int>();
			try
			{
				await GetBearerToken();
				await _client.AuthorsPOSTAsync(author);
			}
			catch (ApiException exception)
			{
				response = ConvertApiExceptions<int>(exception);
			}

			return response;
		}
		public async Task<Response<int>> Edit(int id, AuthorUpdateDto author)
		{
			Response<int> response = new Response<int>();
			try
			{
				await GetBearerToken();
				await _client.AuthorsPUTAsync(id, author);
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
				await _client.AuthorsDELETEAsync(id);
			}
			catch (ApiException exception)
			{
				response = ConvertApiExceptions<int>(exception);
			}

			return response;
		}
	}
}
