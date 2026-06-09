using System.ComponentModel.DataAnnotations.Schema;
namespace mvc.Models;

public partial class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    public DateOnly? BirthDate { get; set; }

    public GenderType? Gender { get; set; }

    public string? About { get; set; }

    public Address? Address { get; set; }

    public string? ImageUrl { get; set; }

    public ICollection<Post> Posts { get; set; } = new List<Post>();

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<React> Reacts { get; set; } = new List<React>();

    public ICollection<Follow> Followers { get; set; } = new List<Follow>();

    public ICollection<Follow> Followings { get; set; } = new List<Follow>();

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
