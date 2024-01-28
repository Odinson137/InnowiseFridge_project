namespace InnowiseFridge_project.Interfaces.ServiceInterfaces;

public interface IFileService
{
    string GetUniqueName(IFormFile file);
    Task CreateFileAsync(IFormFile file, string nameFile);
}