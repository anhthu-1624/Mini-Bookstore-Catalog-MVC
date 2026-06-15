namespace AspNetWeek4.Mvc.Models;
public class SaleItem
{
    public int Id { get; set; }

    public int SaleId { get; set; }

    public Sale? Sale { get; set; }

    public int BookId { get; set; }

    public Book? Book { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Subtotal => Quantity * UnitPrice;
}