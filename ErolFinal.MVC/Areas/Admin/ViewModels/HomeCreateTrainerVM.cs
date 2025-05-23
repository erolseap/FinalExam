using System.ComponentModel.DataAnnotations;

namespace ErolFinal.MVC.Areas.Admin.ViewModels;

public class HomeCreateTrainerVM
{
    [Required]
    [MinLength(3)]
    [MaxLength(95 + 1)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please set the category name")]
    [MinLength(3)]
    [MaxLength(64)]
    public string Category { get; set; }

    [Required(ErrorMessage = "Please set the description name")]
    [MinLength(3)]
    [MaxLength(256)]
    public string Description { get; set; }

    [Required]
    [DataType(DataType.Upload)]
    public IFormFile Img { get; set; }

    // Urls

    [Required]
    [MinLength(5)]
    [MaxLength(2048 + 1)]
    [DataType(DataType.Url)]
    public string FacebookUrl { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(2048 + 1)]
    [DataType(DataType.Url)]
    public string TwitterUrl { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(2048 + 1)]
    [DataType(DataType.Url)]
    public string BehanceUrl { get; set; }
}
