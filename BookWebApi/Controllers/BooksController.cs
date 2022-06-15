using Microsoft.AspNetCore.Mvc;
using BookWebApi.Data;
using BookWebApi.Contracts;
using BookWebApi.Models;
using BookWebApi.QueryParameters;
using BookWebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
namespace BookWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly ILogger<BooksController> _logger;
    private readonly IRepository<Book> _bookRepo;

    private readonly IRepository<Author> _authorRepo;
    public BooksController(
        ILogger<BooksController> logger, 
        IRepository<Book> bookRepo, 
        IRepository<Author> authorRepo)
    {
        _logger = logger;
        _bookRepo = bookRepo;
        _authorRepo = authorRepo;
    }

    [Authorize]
    [HttpPost]
    public IActionResult PublishBook([FromBody] BookInfo book)
    {
        Guid userId = User.Claims
            .Where(c => c.Type == "UserId")
            .Select(c => Guid.Parse(c.Value))
            .FirstOrDefault();
        var newBook = new Book(
            Guid.NewGuid(), 
            book.Title, 
            book.Description, 
            _authorRepo.Get(userId), 
            book.ImageName ?? String.Empty, 
            book.Price, 
            DateTime.Now);
        _bookRepo.Upsert(newBook);
        return Ok();
    }

    [Authorize]
    [HttpDelete]
    public IActionResult UnpublishBook(Guid id)
    {
        _bookRepo.Delete(_bookRepo.Get(id));
        return Ok();
    }

    [HttpGet]
    public IEnumerable<BookInfo> Get([FromQuery] BookQueryParameters parameters)
    {
        var allBooks = _bookRepo.GetAll();

        if (parameters.IsEmptyQueryParameter)
        {
            return allBooks.Select(b => Translate(b)).ToArray();
        }

        var byTitles = parameters.Title == string.Empty ?
            Enumerable.Empty<Book>() :
            allBooks.Where(b => b.Title.StartEndOrEqual(parameters.Title));

        var byFirstNames = parameters.FirstName == string.Empty ?
            Enumerable.Empty<Book>() :
            allBooks.Where(b => b.Author.FirstName.StartEndOrEqual(parameters.FirstName));

        var byLastNames = parameters.LastName == string.Empty ?
            Enumerable.Empty<Book>() :
            allBooks.Where(b => b.Author.LastName.StartEndOrEqual(parameters.LastName));

        return byTitles
            .Concat(byFirstNames)
            .Concat(byLastNames)
            .Select(b => Translate(b))
            .Distinct()
            .ToArray();
    }

    private BookInfo Translate(Book book) =>
        new BookInfo() 
        { 
            Id = book.GetId(), 
            Title = book.Title, 
            Description = book.Description, 
            Price = book.Price, 
            ImageName = book.Image,
            AuthorName = $"{book.Author.FirstName} {book.Author.LastName}"
        };
}
