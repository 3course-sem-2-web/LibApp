namespace task2.DTOs;

public record class GetReviewDto(int Id, string Message, string Reviewer);

//public class GetReviewDto
//{
//    [JsonPropertyName("id")]
//    public int Id { get; }

//    [JsonPropertyName("message")]
//    public string Message { get; }

//    [JsonPropertyName("reviewer")]
//    public string Reviewer { get; }

//    public GetReviewDto(int id, string message, string reviewer)
//    {
//        Id = id;
//        Message = message;
//        Reviewer = reviewer;
//    }
//}
