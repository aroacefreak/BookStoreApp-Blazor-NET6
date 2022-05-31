﻿using BookStoreApp.Blazor.Server.UI.Models;
using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services;

public interface IAuthorService
{
	Task<Response<AuthorReadOnlyDtoVirtualiseResponse>> Get(QueryParameters queryParams);
	Task<Response<List<AuthorReadOnlyDto>>> Get();
	Task<Response<AuthorDetailsDto>> Get(int Id);
	Task<Response<AuthorUpdateDto>> GetForUpdate(int Id);
	Task<Response<int>> Create(AuthorCreateDto author);
	Task<Response<int>> Edit(int id, AuthorUpdateDto author);
	Task<Response<int>> Delete(int id);
}