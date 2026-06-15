using System.ComponentModel.DataAnnotations;

namespace AspNetWeek4.Mvc.ViewModels;

public class SaleCreateViewModel
{
    [Required]
    public int BookId { get; set; }

    [Range(1, 1000)]
    public int Quantity { get; set; }
}