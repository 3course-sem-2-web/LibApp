namespace task2.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Cover { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Genre { get; set; } = null!;

    public ICollection<Review> Reviews { get; set; } = null!;
    public ICollection<Rating> Ratings { get; set; } = null!;
}