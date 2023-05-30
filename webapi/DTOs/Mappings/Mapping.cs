using task2.BLL;
using task2.Models;

namespace task2.DTOs.Mappings;

public static class Mapping
{
    public static BookDetailsDto ToBookDetailsDto(this Book book)
    {
        var result = new BookDetailsDto(
            book.Id,
            book.Title,
            book.Author,
            book.Cover,
            book.Content,
            BooksRepository.EvaluateBookRating(book));

        result.Reviews.AddRange(
            book
            .Reviews
            .Select(x => x.ToReviewDto()));

        return result;
    }

    public static GetBookDto ToGetBookDto(this Book book)
    {
        return new GetBookDto(
            book.Id,
            book.Title,
            book.Author,
            book.Cover,
            BooksRepository.EvaluateBookRating(book),
            book.Genre,
            book.Content,
            book.Reviews.Count);
    }

    public static GetReviewDto ToReviewDto(this Review review)
    {
        return new GetReviewDto(review.Id, review.Message, review.Reviewer);
    }

    public static Review FromGetReviewDto(this GetReviewDto reviewDto)
    {
        return new Review
        {
            Id = reviewDto.Id,
            Message = reviewDto.Message,
            Reviewer = reviewDto.Reviewer,
        };
    }

    public static Review FromCreateReviewDto(this CreateReviewDto reviewDto)
    {
        return new Review
        {
            Message = reviewDto.Message,
            Reviewer = reviewDto.Reviewer,
        };
    }

    public static Book FromSaveBookDto(this SaveANewBookDto bookDto)
    {
        return new Book
        {
            Id = bookDto.Id,
            Title = bookDto.Title,
            Cover = bookDto.Cover,
            Content = bookDto.Content,
            Genre = bookDto.Genre,
            Author = bookDto.Author,
        };
    }
}
