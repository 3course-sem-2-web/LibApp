namespace task2.DTOs;

public record class BookDetailsDto(int Id, string Title, string Author, string Cover, string Content, decimal Rating)
{
    public List<GetReviewDto> Reviews { get; } = new List<GetReviewDto>();
}

//public class BookDetailsDto
//{
//    [JsonPropertyName("id")]
//    public int Id { get; }

//    [JsonPropertyName("title")]
//    public string Title { get; }
    
//    [JsonPropertyName("author")]
//    public string Author { get; }

//    [JsonPropertyName("cover")]
//    public string Cover { get; }

//    [JsonPropertyName("content")]
//    public string Content { get; }

//    [JsonPropertyName("rating")]
//    public decimal Rating { get; }

//    [JsonPropertyName("reviews")]
//    public List<GetReviewDto> Reviews { get; }

//    public BookDetailsDto(int id, string title, string author, string cover, string content, decimal rating)
//    {
//        Id = id;
//        Title = title;
//        Author = author;
//        Cover = cover;
//        Content = content;
//        Rating = rating;
//        Reviews = new List<GetReviewDto>();
//    }
//}
