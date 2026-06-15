// Data/AppDbContext.cs
using AspNetWeek4.Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetWeek4.Mvc.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(100);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Books");

            entity.HasKey(b => b.Id);

            entity.Property(b => b.Isbn)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(b => b.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(b => b.Author)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(b => b.Genre)
                  .HasMaxLength(100);

            entity.Property(b => b.Publisher)
                  .HasMaxLength(150);

            entity.Property(b => b.Price)
                  .HasColumnType("decimal(18,2)");
            entity.Property(b => b.BookCode)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.HasOne(b => b.Category)
                  .WithMany(c => c.Books)
                  .HasForeignKey(b => b.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("Sales");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.TotalAmount)
                  .HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.ToTable("SaleItems");

            entity.HasKey(si => si.Id);

            entity.Property(si => si.UnitPrice)
                  .HasColumnType("decimal(18,2)");

            entity.HasOne(si => si.Sale)
                  .WithMany(s => s.SaleItems)
                  .HasForeignKey(si => si.SaleId);

            entity.HasOne(si => si.Book)
                  .WithMany()
                  .HasForeignKey(si => si.BookId);
        });
        // Seed Categories
    modelBuilder.Entity<Category>().HasData(
        new Category { Id = 1, Name = "Programming" },
        new Category { Id = 2, Name = "Data Science" }
    );

    // Seed Books
    modelBuilder.Entity<Book>().HasData(
        new Book
        {
            Id = 1,
            BookCode = "BK001",
            Isbn = "9780132350884",
            Title = "Clean Code",
            Author = "Robert C. Martin",
            Genre = "Programming",
            Publisher = "Prentice Hall",
            Price = 450000,
            Stock = 10,
            MinStock = 2,
            PublishedDate = new DateTime(2008, 8, 1),
            LastUpdatedAt = new DateTime(2025, 1, 1),
            CategoryId = 1
        },
        new Book
        {
            Id = 2,
            BookCode = "BK002",
            Isbn = "9781617294532",
            Title = "ASP.NET Core in Action",
            Author = "Andrew Lock",
            Genre = "Programming",
            Publisher = "Manning",
            Price = 650000,
            Stock = 5,
            MinStock = 2,
            PublishedDate = new DateTime(2021, 1, 1),
            LastUpdatedAt = new DateTime(2025, 1, 1),
            CategoryId = 1
        },
        
        new Book
      {
            Id = 3,
            BookCode = "BK003",
            Isbn = "9781491950496",
            Title = "Data Science from Scratch",
            Author = "Joel Grus",
            Genre = "Data Science",
            Publisher = "O'Reilly Media",
            Price = 550000,
            Stock = 8,
            MinStock = 3,
            PublishedDate = new DateTime(2019, 4, 1),
            LastUpdatedAt = new DateTime(2025, 1, 1),
            CategoryId = 2
        },
            new Book
            {
                  Id = 4,
                  BookCode = "BK004",
                  Isbn = "9781492078006",
                  Title = "Python for Data Analysis",
                  Author = "Wes McKinney",
                  Genre = "Data Science",
                  Publisher = "O'Reilly Media",
                  Price = 600000,
                  Stock = 6,
                  MinStock = 2,
                  PublishedDate = new DateTime(2018, 10, 1),
                  LastUpdatedAt = new DateTime(2025, 1, 1),
                  CategoryId = 2
            },
            new Book
            {
                  Id = 5,
                  BookCode = "BK005",
                  Isbn = "9781491901427",
                  Title = "Fluent Python",
                  Author = "Luciano Ramalho",
                  Genre = "Programming",
                  Publisher = "O'Reilly Media",
                  Price = 700000,
                  Stock = 4,
                  MinStock = 1,
                  PublishedDate = new DateTime(2015, 8, 1),
                  LastUpdatedAt = new DateTime(2025, 1, 1),
                  CategoryId = 1
            },
            new Book
            {
                  Id = 6,
                  BookCode = "BK006",
                  Isbn = "9781491957660",
                  Title = "Python Data Science Handbook",
                  Author = "Jake VanderPlas",
                  Genre = "Data Science",
                  Publisher = "O'Reilly Media",
                  Price = 650000,
                  Stock = 7,
                  MinStock = 2,
                  PublishedDate = new DateTime(2016, 11, 1),
                  LastUpdatedAt = new DateTime(2025, 1, 1),
                  CategoryId = 2
            },
            new Book
            {
                  Id = 7,
                  BookCode = "BK007",
                  Isbn = "9781492078005",
                  Title = "Effective C#",
                  Author = "Bill Wagner",
                  Genre = "Programming",
                  Publisher = "Pearson",
                  Price = 550000,
                  Stock = 5,
                  MinStock = 2,
                  PublishedDate = new DateTime(2020, 5, 1),
                  LastUpdatedAt = new DateTime(2025, 1, 1),
                  CategoryId = 1
            },
            new Book
            {
                  Id = 8,
                  BookCode = "BK008",
                  Isbn = "9781491950296",
                  Title = "Data Science for Business",
                  Author = "Foster Provost",
                  Genre = "Data Science",
                  Publisher = "O'Reilly Media",
                  Price = 600000,
                  Stock = 6,
                  MinStock = 2,
                  PublishedDate = new DateTime(2013, 8, 1),
                  LastUpdatedAt = new DateTime(2025, 1, 1),
                  CategoryId = 2
            });   
      }
}