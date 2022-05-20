using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Books;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
   public class BooksController : ControllerBase
    {
	    private readonly BookStoreDbContext _context;
		private readonly IMapper _mapper;

		public BooksController(BookStoreDbContext context, IMapper mapper)
		{ 
	      _context = context;
			_mapper = mapper;
		}

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
          if (_context.Books == null)
          {
              return NotFound();
          }

          var booksDtos = await _context.Books
	          .Include(q => q.Author)
	          .ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider)
	          .ToListAsync();

          return Ok(booksDtos);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        { 
	        if (_context.Books == null) 
	        { 
		        return NotFound();
	        } 
	        var bookDto = await _context.Books
		        .Include(q => q.Author)
		        .ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider)
		        .FirstOrDefaultAsync(q => q.Id == id);

	        
	        if (bookDto == null) 
	        { 
		        return NotFound();
	        }
	        
	        return bookDto;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administration")]
      public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
	            return NotFound();
            }

            _mapper.Map(bookDto, book);
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administration")]
      public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
        { 
	        if (_context.Books == null) 
	        { 
		        return Problem("Entity set 'BookStoreDbContext.Books'  is null.");
	        }
	        
	        var book = _mapper.Map<Book>(bookDto);
	        
	        await _context.Books.AddAsync(book); 
	        await _context.SaveChangesAsync();
	        
	        return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administration")]
      public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
