namespace task2.DTOs;

public record class SaveANewBookDto(
    int Id ,
    string Title ,
    string Cover ,
    string Content ,
    string Genre ,
    string Author);

//public class SaveANewBookDto
//{
//    public SaveANewBookDto(int id, string title, string cover, string content, string genre, string author)
//    {
//        Id = id;
//        Title = title;
//        Cover = cover;
//        Content = content;
//        Genre = genre;
//        Author = author;
//    }

//    [JsonPropertyName("id")]
//    public int Id { get; }

//    [JsonPropertyName("title")]
//    public string Title { get; }

//    [JsonPropertyName("cover")]
//    public string Cover { get; }

//    [JsonPropertyName("content")]
//    public string Content { get; }

//    [JsonPropertyName("genre")]
//    public string Genre { get; }

//    [JsonPropertyName("author")]
//    public string Author { get; }

//}
