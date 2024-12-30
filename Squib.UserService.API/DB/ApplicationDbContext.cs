using Microsoft.EntityFrameworkCore;
using Squib.UserService.API.Model;

namespace Squib.UserService.API.DB
{
    public class ApplicationDbContext : DbContext
    {
        // Define DbSet for each table
        public DbSet<UserDto> UserDtos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Configure entity relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map the UserDto entity to the UserDto_New table
            modelBuilder.Entity<UserDto>(entity =>
            {
                entity.ToTable("UserDto_New"); // Table name in the database
                entity.HasKey(e => e.Id); // Primary key

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(255); // Email column is required and has a max length of 255

                entity.Property(e => e.FirstName)
                      .IsRequired()
                      .HasMaxLength(255); // FirstName column is required

                entity.Property(e => e.LastName)
                      .IsRequired()
                      .HasMaxLength(255); // LastName column is required

                // Optional: Add a unique constraint for the Email column
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}