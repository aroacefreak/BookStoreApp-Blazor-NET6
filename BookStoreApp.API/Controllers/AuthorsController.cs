using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Static;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
	        _context = context;
	        _mapper = mapper;
	        _logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
           _logger.LogInformation($"Request to {nameof(GetAuthors)}");
	        try
	        {
		        if (_context.Authors == null)
		        {
			        return NotFound();
		        }

		        var authors = await _context.Authors.ToListAsync();
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
        public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
        {
	        _logger.LogInformation($"Request to {nameof(GetAuthor)}");
	        try
	        {
		        if (_context.Authors == null)
		        {
			        return NotFound();
		        }
		        var author = await _context.Authors.FindAsync(id);

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
		        var author = await _context.Authors.FindAsync(id);

		        if (author == null) 
		        {
					_logger.LogWarning($"{nameof(Author)} record not found in {nameof(PutAuthor)} - ID: {id}");
					return NotFound();
		        }

		        _mapper.Map(authorDto, author);
		        _context.Entry(author).State = EntityState.Modified;

		        try
		        {
			        await _context.SaveChangesAsync();
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
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
	        _logger.LogInformation($"Request to {nameof(PostAuthor)}");
	        try
	        {
		        if (_context.Authors == null)
		        { 
			        return Problem("Entity set 'BookStoreDbContext.Authors'  is null.");
		        }

		        var author = _mapper.Map<Author>(authorDto);

		        await _context.Authors.AddAsync(author);
		        await _context.SaveChangesAsync();

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
        public async Task<IActionResult> DeleteAuthor(int id)
        {
	        _logger.LogInformation($"Request to {nameof(DeleteAuthor)}");
	        try
	        { 
		        if (_context.Authors == null) 
		        { 
			        _logger.LogError($"{nameof(Author)} collection doesn't exist in Database"); 
			        return NotFound();
		        } 
		        var author = await _context.Authors.FindAsync(id); 
		        if (author == null) 
		        {
			        _logger.LogError($"{nameof(Author)} record not found in {nameof(DeleteAuthor)} - ID: {id}"); 
			        return NotFound();
		        }
		        
		        _context.Authors.Remove(author); 
		        await _context.SaveChangesAsync();
		        
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
	        return await _context.Authors?.AnyAsync(e => e.Id == id);
        }
    }
}
