namespace mvc.ViewModels;

public class EditVM
{
    public string FirstName { get; set; }

    public string LasttName { get; set; }

    public string UserName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public GenderType? Gender { get; set; }

    public string? About { get; set; }

    public Address? Address { get; set; }

    public IFormFile? Image { get; set; }

    public string? ImageUrl { get; set; }
}
