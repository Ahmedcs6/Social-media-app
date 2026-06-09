namespace mvc.Models;

public class Post
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime PublishDate { get; set; }

    public string? ImageUrl { get; set; }

    public PrivacyType Privacy { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<React> Reacts { get; set; } = new List<React>();

    public string UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
