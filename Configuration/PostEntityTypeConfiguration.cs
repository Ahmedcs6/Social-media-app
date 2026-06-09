using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace mvc.Configuration;

public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Post");
        builder.Property(p => p.Content)
                       .IsRequired()
                       .HasMaxLength(5000);

        builder.Property(p => p.PublishDate)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(p => p.User)
               .WithMany(u => u.Posts)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
