using BookAPI.Data;
using BookAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BookAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookContext _context;

    public BooksController(BookContext context)
    {
        _context = context;
    }

    // GET api/books
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        return Ok(_context.Books.ToList());
    }

    // GET api/books/1
    [HttpGet("{id}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = _context.Books.Find(id);
        if (book is null)
            return NotFound();
        return Ok(book);
    }

    // POST api/books
    [HttpPost]
    public ActionResult<Book> Create(Book book)
    {
        try
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }
        catch (DbUpdateException)
        {
            return BadRequest("Could not create the book.");
        }

        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT api/books/1
    [HttpPut("{id}")]
    public IActionResult Update(int id, Book updatedBook)
    {
        if (id != updatedBook.Id)
            return BadRequest();

        //  _context.Entry(updatedBook).State = EntityState.Modified;

        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            if (!_context.Books.Any(b => b.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // PATCH api/books/1
    [HttpPatch("{id}")]
    public IActionResult Patch(int id, [FromBody] Dictionary<string, object> patchData)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book is null)
            return NotFound();

        if (patchData.ContainsKey("Title"))
            book.Title = patchData["Title"]?.ToString()!;
        if (patchData.ContainsKey("Author"))
            book.Author = patchData["Author"]?.ToString()!;
        if (patchData.ContainsKey("Year") && int.TryParse(patchData["Year"]?.ToString(), out var year))
            book.Year = year;

        _context.SaveChanges();
        return Ok(book);
    }

    // DELETE api/books/1
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var book = _context.Books.Find(id);
        if (book is null)
            return NotFound();

        _context.Books.Remove(book);
        _context.SaveChanges();

        return NoContent();
    }
}
