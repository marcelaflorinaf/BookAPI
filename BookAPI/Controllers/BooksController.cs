using BookAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private static List<Book> books = new()
    {
        new Book { Id = 1, Title = "The Hobbit", Author = "J.R.R. Tolkien", Year = 1937 },
        new Book { Id = 2, Title = "1984", Author = "George Orwell", Year = 1949 }
    };

    // GET api/books
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        return Ok(books);
    }

    // GET api/books/1
    [HttpGet("{id}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        if (book is null)
            return NotFound();
        return Ok(book);
    }

    // POST api/books
    [HttpPost]
    public ActionResult<Book> Create(Book book)
    {
        book.Id = books.Max(b => b.Id) + 1;
        books.Add(book);
        //return 201 Created
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT api/books/1
    [HttpPut("{id}")]
    public IActionResult Update(int id, Book updatedBook)
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        if (book is null)
            return NotFound();

        book.Title = updatedBook.Title;
        book.Author = updatedBook.Author;
        book.Year = updatedBook.Year;

        return NoContent(); // 204
    }

    // PATCH api/books/1
    [HttpPatch("{id}")]
    public IActionResult Patch(int id, [FromBody] Dictionary<string, object> patchData)
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        if (book is null)
            return NotFound();

        if (patchData.ContainsKey("Title"))
            book.Title = patchData["Title"]?.ToString()!;
        if (patchData.ContainsKey("Author"))
            book.Author = patchData["Author"]?.ToString()!;
        if (patchData.ContainsKey("Year") && int.TryParse(patchData["Year"]?.ToString(), out var year))
            book.Year = year;

        return Ok(book);
    }

    // DELETE api/books/1
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        if (book is null)
            return NotFound();

        books.Remove(book);
        return NoContent();
    }
}
