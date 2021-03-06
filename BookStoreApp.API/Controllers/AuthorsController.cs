using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Repositories;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
	public class AuthorsController : ControllerBase
    {
        private readonly IAuthorsRepository _authorsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IAuthorsRepository authorsRepository, IMapper mapper, ILogger<AuthorsController> logger)
        {
	        _authorsRepository = authorsRepository;
	        _mapper = mapper;
	        _logger = logger;
        }

        // GET: api/Authors/?startindex=0&pagesize=15
        [HttpGet]
        public async Task<ActionResult<VirtualiseResponse<AuthorReadOnlyDto>>> GetAuthors([FromQuery]QueryParameters queryParameters)
        {
           _logger.LogInformation($"Request to {nameof(GetAuthors)}");
	        try
	        {
		        var authorDtos = await _authorsRepository.GetAllAsync<AuthorReadOnlyDto>(queryParameters);
		        //var authorDtos = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
		        return Ok(authorDtos);
	        }
	        catch (Exception e)
	        {
		        _logger.LogError(e, $"Error Performing GET in {nameof(GetAuthors)}");
		        return StatusCode(500, Messages.Error500Message);
	        }
         
        }

        // GET: api/Authors/
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<AuthorReadOnlyDto>>> GetAuthors()
        {
	        _logger.LogInformation($"Request to {nameof(GetAuthors)}");
	        try
	        {
		        var authors = await _authorsRepository.GetAllAsync();
		        var authorDtos = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
		        return Ok(authorDtos);
	        } 
	        catch (Exception e)
	        {
		        _logger.LogError(e, $"Error Performing GET in {nameof(GetAuthors)}");
		        return StatusCode(500, Messages.Error500Message);
	        }

        }

		// GET: api/Authors/5
		[HttpGet("{id}")]
        public async Task<ActionResult<AuthorDetailsDto>> GetAuthor(int id)
        {
	        _logger.LogInformation($"Request to {nameof(GetAuthor)}");
	        try
	        {
		        var author = await _authorsRepository.GetAuthorDetailsAsync(id);

		        if (author == null)
		        {
			        _logger.LogWarning($"Record Not Found: {nameof(GetAuthor)} - ID: {id}");
			        return NotFound();
		        }

		        var authorDto = _mapper.Map<AuthorReadOnlyDto>(author);
		        //Return code included in what's being returned
		        return Ok(authorDto);
	        }
	        catch (Exception e)
	        {
		        _logger.LogError(e, $"Error Performing GET in {nameof(GetAuthor)}");
		        return StatusCode(500, Messages.Error500Message);
	        }
         
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administration")]
		public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
	        _logger.LogInformation($"Request to {nameof(PutAuthor)}");
	        try
	        {
		        if (id != authorDto.Id)
		        {
			        _logger.LogWarning($"Update ID invalid in {nameof(PutAuthor)} - ID: {id}");
					return BadRequest();
		        }

		        //This is different
		        var author = await _authorsRepository.GetAsync(id);

		        if (author == null) 
		        {
					_logger.LogWarning($"{nameof(Author)} record not found in {nameof(PutAuthor)} - ID: {id}");
					return NotFound();
		        }

		        _mapper.Map(authorDto, author);

		        try
		        { 
			        await _authorsRepository.UpdateAsync(author);
		        }
		        catch (DbUpdateConcurrencyException)
		        {
			        if (!await AuthorExists(id))
			        {
				        return NotFound();
			        }
			        else
			        {
				        throw;
			        }
		        }

		        return NoContent();
	        }
	        catch (Exception e)
	        {
		        _logger.LogError(e, $"Error Performing PUT in {nameof(PutAuthor)}");
		        return StatusCode(500, Messages.Error500Message);
	        }
         
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administration")]
		public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
	        _logger.LogInformation($"Request to {nameof(PostAuthor)}");
	        try
	        {
		        var author = _mapper.Map<Author>(authorDto);

		        await _authorsRepository.AddAsync(author);


				  return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
	        }
	        catch (Exception e)
	        {
		        _logger.LogError(e, $"Error Performing Post in {nameof(PostAuthor)}");
		        return StatusCode(500, Messages.Error500Message);
	        }
			
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administration")]
		public async Task<IActionResult> DeleteAuthor(int id)
        {
	        _logger.LogInformation($"Request to {nameof(DeleteAuthor)}");
	        try
	        { 
		        var author = await _authorsRepository.GetAsync(id); 
		        if (author == null) 
		        {
			        _logger.LogError($"{nameof(Author)} record not found in {nameof(DeleteAuthor)} - ID: {id}"); 
			        return NotFound();
		        }
		        
		        await _authorsRepository.DeleteAsync(id);


				  return NoContent();
	        }
	        catch (Exception e)
	        {
		        _logger.LogError(e, $"Error Performing Delete in {nameof(DeleteAuthor)}");
		        return StatusCode(500, Messages.Error500Message);
	        }
        }

        private async Task<bool> AuthorExists(int id)
        { 
	        return await _authorsRepository.Exists(id);
        }
    }
}
