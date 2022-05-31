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
using BookStoreApp.API.Models;
using BookStoreApp.API.Models.Books;
using BookStoreApp.API.Repositories;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
   public class BooksController : ControllerBase
    {
	    private readonly IBooksRepository _booksRepository;
	    private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly ILogger<AuthorsController> _logger;

		public BooksController(IBooksRepository booksRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<AuthorsController> logger)
		{ 
	      _booksRepository = booksRepository;
	      _mapper = mapper;
			_webHostEnvironment = webHostEnvironment;
			_logger = logger;
		}

      // GET: api/Books/?startindex=0&pagesize=15
      [HttpGet]
		public async Task<ActionResult<VirtualiseResponse<BookReadOnlyDto>>> GetBooks([FromQuery] QueryParameters queryParameters)
		{
			try
			{
				var booksDtos = await _booksRepository.GetAllAsync<BookReadOnlyDto>(queryParameters);
				return Ok(booksDtos);
         }
			catch (Exception e)
			{
				_logger.LogError(e, $"Error Performing GET in {nameof(GetBooks)}");
				return StatusCode(500, Messages.Error500Message);
			}
      }

      // GET: api/Books
      [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
	        var booksDtos = await _booksRepository.GetAllBooksAsync();
	        return Ok(booksDtos);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        { 
	        var bookDto = await _booksRepository.GetBookAsync(id);

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

            var book = await _booksRepository.GetAsync(id);

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

            try
            {
                await _booksRepository.UpdateAsync(book);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookExists(id))
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
	        var book = _mapper.Map<Book>(bookDto);
	        book.Image = CreateFile(bookDto.ImageData, bookDto.OriginalImageName);
	        
	        await _booksRepository.AddAsync(book);
	        
	        return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administration")]
      public async Task<IActionResult> DeleteBook(int id)
        { 
	        var book = await _booksRepository.GetAsync(id);
	        if (book == null)
	        {
		        return NotFound();
	        }
           
	        await _booksRepository.DeleteAsync(id);

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

        private async Task<bool> BookExists(int id)
        {
	        return await _booksRepository.Exists(id);
        }
    }
}
