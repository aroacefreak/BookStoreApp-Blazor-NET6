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
		private readonly IWebHostEnvironment _webHostEnvironment;

		public BooksController(BookStoreDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
		{ 
	      _context = context;
			_mapper = mapper;
			_webHostEnvironment = webHostEnvironment;
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

            if (!string.IsNullOrEmpty((bookDto.ImageData)))
            {
	            bookDto.Image = CreateFile(bookDto.ImageData, bookDto.OriginalImageName);

	            var picName = Path.GetFileName(book.Image);
	            var path = $"{_webHostEnvironment.WebRootPath}\\bookcoverimages\\{picName}";
	            if (System.IO.File.Exists(path))
	            {
                  System.IO.File.Delete(path);
	            }
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
	        book.Image = CreateFile(bookDto.ImageData, bookDto.OriginalImageName);
	        
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

      private string CreateFile(string imageBase64, string imageName)
      {
	      var url = HttpContext.Request.Host.Value;
	      var ext = Path.GetExtension(imageName);
	      var fileName = $"{Guid.NewGuid()}{ext}";
	      var path = $"{_webHostEnvironment.WebRootPath}\\bookcoverimages\\{fileName}";

	      byte[] image = Convert.FromBase64String(imageBase64);

	      var fileStream = System.IO.File.Create(path);
         fileStream.Write(image, 0, image.Length);
         fileStream.Close();

         return $"https://{url}/bookcoverimages/{fileName}";
      }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
