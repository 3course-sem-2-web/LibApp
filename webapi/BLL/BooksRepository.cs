using Microsoft.EntityFrameworkCore;
using task2.BLL.Exceptions;
using task2.DTOs;
using task2.DTOs.Mappings;
using task2.Models;
using webapi.Context;

namespace task2.BLL;

public class BooksRepository
{
    private const string SecretSectionName = "secret";

    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public BooksRepository(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<List<GetBookDto>> GetAllBooksOrderedByTitle()
    {
        var result = await _context.Books
            .OrderBy(x => x.Title)
            .Include(x=> x.Ratings)
            .Include(x=> x.Reviews)
            .Select(x=> x.ToGetBookDto())
            .ToListAsync();
        return result;
    }

    public async Task<List<GetBookDto>> GetAllBooksOrderedByAuthor()
    {
        var result = await _context.Books
            .OrderBy(x => x.Author)
            .Include(x=> x.Ratings)
            .Include(x=> x.Reviews)
            .Select(x => x.ToGetBookDto())
            .ToListAsync();
        return result;
    }

    public List<GetBookDto> GetTopByGenre(string? genre)
    {
        const int amountOfItemsOnTop = 10;
        const int reviewsAmountToBeGreaterThen = 10;

        var query = _context.Books
            .Include(x => x.Reviews)
            .Include(x => x.Ratings)
            .OrderByDescending(EvaluateBookRating)
            .Where(x => x.Reviews.Count > reviewsAmountToBeGreaterThen);

        if (genre != null)
        {
            query = query.Where(x => x.Genre == genre);
        }

        query = query.Take(amountOfItemsOnTop);

        return query
            .Select(x=> x.ToGetBookDto())
            .ToList();
    }

    public async Task<BookDetailsDto> GetBookDetails(int bookId)
    {
        var foundBook = await FindBook(bookId);
        await _context
            .Entry(foundBook)
            .Collection(x => x.Ratings)
            .LoadAsync();
        await _context
            .Entry(foundBook)
            .Collection(x => x.Reviews)
            .LoadAsync();
        return foundBook.ToBookDetailsDto();
    }

    public async Task DeleteBook(int bookId, string secret)
    {
        if (secret != _configuration.GetValue<string>(SecretSectionName))
        {
            throw new DeleteAccessDeniedException();
        }

        var foundBookToDelete = await FindBook(bookId);
        
        _context.Books.Remove(foundBookToDelete);

        await _context.SaveChangesAsync();
    }

    public async Task<int> SaveNewBook(SaveANewBookDto saveBookDto)
    {
        var book = saveBookDto.FromSaveBookDto();

        var bookDoesExist = await _context
            .Books
            .AnyAsync(x => saveBookDto.Id == x.Id);

        if (book.Id == 0 || !bookDoesExist)
        {
            _context.Add(book);
        }
        else
        {
            _context.Update(book);
        }

        await _context.SaveChangesAsync();

        return book.Id;
    }

    public async Task<int> SaveReviewForTheBook(int bookId, CreateReviewDto reviewDto)
    {
        var reviewToAssignToBook = reviewDto.FromCreateReviewDto();

        var foundBook = await FindBook(bookId);
        
        await _context.Entry(foundBook)
            .Collection(x=> x.Reviews)
            .LoadAsync();
        
        foundBook.Reviews.Add(reviewToAssignToBook);

        await _context.SaveChangesAsync();
        
        return reviewToAssignToBook.Id;
    }

    public async Task RateBook(int bookId, RateBookDto requestBody)
    {
        var foundBook = await FindBook(bookId);

        await _context
            .Entry(foundBook)
            .Collection(x => x.Ratings)
            .LoadAsync();

        foundBook.Ratings.Add(new Rating
        {
            Score = requestBody.Score,
        });

        await _context.SaveChangesAsync();
    }

    private async Task<Book> FindBook(int bookId)
    {
        var foundBook = await _context.Books.SingleOrDefaultAsync(x => x.Id == bookId);

        if (foundBook == null)
        {
            throw new BookNotFoundException();
        }

        return foundBook;
    }

    public static decimal EvaluateBookRating(Book book) => 
        book.Ratings.Count switch
    {
        > 0 => book.Ratings.Average(x => x.Score),
        _ => 0
    };
}
