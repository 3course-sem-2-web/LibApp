namespace task2.DTOs;

public record class CreateReviewDto(string Message, string Reviewer);

//public class CreateReviewDto
//{
//    public CreateReviewDto(string message, string reviewer)
//    {
//        Message = message;
//        Reviewer = reviewer;
//    }

//    [JsonPropertyName("message")]
//    public string Message { get; }

//    [JsonPropertyName("reviewer")]
//    public string Reviewer { get; }

//}
