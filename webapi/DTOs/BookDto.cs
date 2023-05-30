namespace task2.DTOs;

public record class GetBookDto(
    int Id,
    string Title, 
    string Author, 
    string Cover, 
    decimal Rating,
    string Genre,
    string Content,
    int ReviewsNumber);

//public class GetBookDto
//{
//    public GetBookDto(int id, string title, string author, decimal rating, int reviewsNumber)
//    {
//        Id = id;
//        Title = title;
//        Author = author;
//        Rating = rating;
//        ReviewsNumber = reviewsNumber;
//    }

//    [JsonPropertyName("id")]
//    public int Id { get; }

//    [JsonPropertyName("title")]
//    public string Title { get; }

//    [JsonPropertyName("author")]
//    public string Author { get; }

//    [JsonPropertyName("rating")]
//    public decimal Rating { get; }

//    [JsonPropertyName("reviewsNumber")]
//    public int ReviewsNumber { get; }

//}