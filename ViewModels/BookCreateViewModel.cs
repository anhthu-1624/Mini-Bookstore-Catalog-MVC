using System.ComponentModel.DataAnnotations;

namespace AspNetWeek4.Mvc.ViewModels;

public class BookCreateViewModel
{
    [Required(ErrorMessage = "ISBN không được để trống.")]
    [StringLength(20, ErrorMessage = "ISBN tối đa 20 ký tự.")]
    public string Isbn { get; set; } = "";

    [Required(ErrorMessage = "Mã sách không được để trống.")]
    [StringLength(20, ErrorMessage = "Mã sách tối đa 20 ký tự.")]
    public string BookCode { get; set; } = "";

    [Required(ErrorMessage = "Tên sách không được để trống.")]
    [StringLength(200, ErrorMessage = "Tên sách tối đa 200 ký tự.")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Tác giả không được để trống.")]
    [StringLength(100, ErrorMessage = "Tên tác giả tối đa 100 ký tự.")]
    public string Author { get; set; } = "";

    [Required(ErrorMessage = "Thể loại không được để trống.")]
    public string Genre { get; set; } = "";

    [Required(ErrorMessage = "Nhà xuất bản không được để trống.")]
    public string Publisher { get; set; } = "";

    [Required(ErrorMessage = "Giá bán không được để trống.")]
    [Range(1, 10_000_000, ErrorMessage = "Giá bán phải lớn hơn 0.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm.")]
    public int Stock { get; set; } = 1;

    [Range(0, int.MaxValue, ErrorMessage = "Mức tồn tối thiểu không được âm.")]
    public int MinStock { get; set; } = 5;

    [Required(ErrorMessage = "Ngày xuất bản không được để trống.")]
    public DateTime PublishedDate { get; set; } = DateTime.Today;
    [Required(ErrorMessage = "Danh mục không được để trống.")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục.")]
    public int CategoryId { get; set; }
}