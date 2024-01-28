using InnowiseFridge_project.Interfaces.ServiceInterfaces;

namespace InnowiseFridge_project.Services;

public class FileService : IFileService
{
    public string GetUniqueName(IFormFile file)
    {
        return $"Image_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(file.FileName)}";
    }
    
    public async Task CreateFileAsync(IFormFile file, string nameFile)
    {
        await using var stream = File.Create($@"./wwwroot/{nameFile}");
        await file.CopyToAsync(stream);
    }
}