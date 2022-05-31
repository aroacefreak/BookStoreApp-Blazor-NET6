﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
	private readonly BookStoreDbContext _context;
	private readonly IMapper _mapper;

	public GenericRepository(BookStoreDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}
	public async Task<T> AddAsync(T entity)
	{
		await _context.AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task DeleteAsync(int id)
	{
		var entity = await GetAsync(id);
		_context.Set<T>().Remove(entity);
		await _context.SaveChangesAsync();
	}

	public async Task<List<T>> GetAllAsync()
	{
		return await _context.Set<T>().ToListAsync();
	}

	public async Task<T> GetAsync(int? id)
	{
		if (id == null)
		{
			return null;
		}

		return await _context.Set<T>().FindAsync(id);
	}

	public async Task UpdateAsync(T entity)
	{
		_context.Update(entity);
		await _context.SaveChangesAsync();
	}

	public async Task<bool> Exists(int id)
	{
		var entity = await GetAsync(id);
		return entity != null;
	}

	public async Task<VirtualiseResponse<TResult>> GetAllAsync<TResult>(QueryParameters queryParam) where TResult : class
	{
		var totalSize = await _context.Set<T>().CountAsync();
		var items = await _context.Set<T>()
			.Skip(queryParam.StartIndex)
			.Take(queryParam.PageSize)
			.ProjectTo<TResult>(_mapper.ConfigurationProvider)
			.ToListAsync();

		return new VirtualiseResponse<TResult> { Items = items, TotalSize = totalSize };
	}
}