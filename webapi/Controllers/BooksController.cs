using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Shared;
using System.Text.Json;
using task2.BLL;
using task2.DTOs;

namespace task2.Controllers;


//[Authorize(
//    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, 
//    Roles =$"{LibraryAppRoles.Admin},{LibraryAppRoles.Manager},{LibraryAppRoles.User}")]

public class BooksController : ApiBase
{
    private readonly BooksRepository _booksRepository;

    public BooksController(BooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<GetBookDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<GetBookDto>>> GetAllBooksAsync([FromQuery] string order)
    {
        if (order == "title")
        {
            return await _booksRepository.GetAllBooksOrderedByTitle();
        }
        else if (order == "author")
        {
            return  await _booksRepository.GetAllBooksOrderedByAuthor();
        }

        return NoContent();
    }

    [HttpGet("api/recomended")]
    [ProducesResponseType(typeof(List<GetBookDto>), StatusCodes.Status200OK)]
    public ActionResult<List<GetBookDto>> GetRecomendedBookAsync([FromQuery] string? genre)
    {
        var topBooks = _booksRepository.GetTopByGenre(genre);
        return topBooks;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookDetailsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<BookDetailsDto>> GetBookDetailsAsync(int id)
    {
        return await _booksRepository.GetBookDetails(id);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBookAsync(int id, [FromQuery] string secret)
    {
        await _booksRepository.DeleteBook(id, secret);
        return NoContent();
    }

    [HttpPost("save")]
    public async Task<string> SaveNewBook([FromBody]SaveANewBookDto requestBody)
    {
        var bookId = await _booksRepository.SaveNewBook(requestBody);
        return JsonSerializer.Serialize(new { Id = bookId });
    }

    [HttpPut("{id:int}/review")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = LibraryAppRoles.Manager)]
    public async Task<string> SaveReview(int id, [FromBody] CreateReviewDto requestBody)
    {
        var reviewForBookId = await _booksRepository.SaveReviewForTheBook(id, requestBody);
        return JsonSerializer.Serialize(new { Id = reviewForBookId });
    }

    [HttpPut("{id:int}/rate")]
    public async Task<IActionResult> RateBookAsync(int id, [FromBody] RateBookDto requestBody)
    {
        await _booksRepository.RateBook(id, requestBody);
        return NoContent();
    }

}
