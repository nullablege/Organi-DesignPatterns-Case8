using System.ComponentModel.DataAnnotations;

namespace Case8.Web.Areas.Admin.Models;

public class ProductFormViewModel
{
    public int Id { get; set; }
    [Required, StringLength(160)]
    public string Name { get; set; } = string.Empty;
    [Required, StringLength(2000)]
    public string Description { get; set; } = string.Empty;
    [Range(0, 999999)]
    public decimal Price { get; set; }
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}
