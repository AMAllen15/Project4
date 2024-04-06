using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_3.Models;

namespace Project_3.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Project_3.Models.Book> Book { get; set; } = default!;
        public DbSet<Project_3.Models.Borrowing> Borrowing { get; set; } = default!;
        public DbSet<Project_3.Models.Reader> Reader { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Borrowings)
                .WithOne(b => b.Book)
                .HasForeignKey(b => b.BookId);

            modelBuilder.Entity<Reader>()
                .HasMany(r => r.Borrowings)
                .WithOne(b => b.Reader)
                .HasForeignKey(b => b.ReaderId);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Readers)
                .WithMany(r => r.Books)
                .UsingEntity(j => j.ToTable("ReaderBooks"));

            modelBuilder.Entity<Reader>()
                .HasMany(r => r.Books)
                .WithMany(b => b.Readers)
                .UsingEntity(j => j.ToTable("ReaderBooks"));
        }
    }
}