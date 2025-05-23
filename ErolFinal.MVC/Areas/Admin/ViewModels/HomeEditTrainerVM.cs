using System.ComponentModel.DataAnnotations;

namespace ErolFinal.MVC.Areas.Admin.ViewModels;

public class HomeEditTrainerVM
{
    [Required]
    public int Id { get; set; }

    [MinLength(3)]
    [MaxLength(95 + 1)]
    public string? Name { get; set; }

    [MinLength(3)]
    [MaxLength(64)]
    public string? Category { get; set; }

    [MinLength(3)]
    [MaxLength(256)]
    public string? Description { get; set; }

    [DataType(DataType.Upload)]
    public IFormFile? Img { get; set; }

    // Urls

    [MinLength(5)]
    [MaxLength(2048 + 1)]
    [DataType(DataType.Url)]
    public string? FacebookUrl { get; set; }

    [MinLength(5)]
    [MaxLength(2048 + 1)]
    [DataType(DataType.Url)]
    public string? TwitterUrl { get; set; }

    [MinLength(5)]
    [MaxLength(2048 + 1)]
    [DataType(DataType.Url)]
    public string? BehanceUrl { get; set; }
}
