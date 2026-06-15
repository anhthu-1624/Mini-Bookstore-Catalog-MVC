namespace AspNetWeek4.Mvc.Models;

public class Book
{
    public int Id { get; set; }

    public string Isbn { get; set; } = "";

    public string Title { get; set; } = "";

    public string Author { get; set; } = "";

    public string Genre { get; set; } = "";

    public string Publisher { get; set; } = "";

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int MinStock { get; set; }

    public DateTime PublishedDate { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    // Foreign Key
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
    public string BookCode { get; set; } = "";
}