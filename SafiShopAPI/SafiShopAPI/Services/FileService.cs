using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SafiShopAPI.Services;

public interface IFileService
{
    Task<string> SaveImageAsync(IFormFile file);
}

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    
    private const string FolderName = "uploads/products";

    public FileService(IWebHostEnvironment env) => _env = env;

    public async Task<string> SaveImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0) return null!;

        var rootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadPath = Path.Combine(rootPath, FolderName);

        if (!Directory.Exists(uploadPath)) 
            Directory.CreateDirectory(uploadPath);

        
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadPath, fileName);

        
        using var image = await Image.LoadAsync(file.OpenReadStream());
        
        
        image.Mutate(x => x.Resize(new ResizeOptions {
            Size = new Size(800, 0),
            Mode = ResizeMode.Max
        }));

        await image.SaveAsync(filePath);

        return $"/{FolderName}/{fileName}";
    }
}