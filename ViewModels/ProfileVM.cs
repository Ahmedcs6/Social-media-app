namespace mvc.ViewModels;

public class ProfileVM
{
    public string? FullName { get; set; }

    public string? UserName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public GenderType? Gender { get; set; }

    public string? About { get; set; }

    public Address? Address { get; set; }

    public string? ImageUrl { get; set; }

    // public ICollection<Post> Posts { get; set; } = new List<Post>();

    public int FollowersNumber { get; set; }

    public int FollowingsNumber { get; set; }

    public bool IsFollowing { get; set; }
}
