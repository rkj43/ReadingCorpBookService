using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadingCorp;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReadingCorpy.Controllers
{
    //https://learn.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5
    public class BookDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

    }
    //https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookContext _context;

        public BooksController(BookContext context)
        {
            _context = context;
        }

        // POST: api/book/create
        [HttpPost("create")]
        public async Task<ActionResult<int>> CreateBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            book.RegistrationTimestamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book.Id;
        }

        // PUT: api/book/{bookId}/update
        [HttpPut("{bookId}/update")]
        public async Task<IActionResult> UpdateBook(int bookId, Book bookUpdateModel)
        {
            if (bookUpdateModel == null)
            {
                return BadRequest();
            }

            var dbBook = await _context.Books.FindAsync(bookId);

            if (dbBook == null)
            {
                return NotFound();
            }

            if (bookUpdateModel.Name != null)
            {
                dbBook.Name = bookUpdateModel.Name;
            }

            if (bookUpdateModel.Author != null)
            {
                dbBook.Author = bookUpdateModel.Author;
            }

            if (bookUpdateModel.Category != null)
            {
                dbBook.Category = bookUpdateModel.Category;
            }

            if (bookUpdateModel.Description != null)
            {
                dbBook.Description = bookUpdateModel.Description;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(bookId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(dbBook); // returning the updated book back to the client
        }
        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            return await _context.Books
                .Select(b => new BookDTO { Id = b.Id, Name = b.Name, Category = b.Category })
                .ToListAsync();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
