namespace ErolFinal.DAL.Models;

public class TrainerModel : BaseModel
{
    public required string Name { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImgFilename { get; set; } = string.Empty;

    // Urls

    public string FacebookUrl { get; set; } = "#";
    public string TwitterUrl { get; set; } = "#";
    public string BehanceUrl { get; set; } = "#";
}
