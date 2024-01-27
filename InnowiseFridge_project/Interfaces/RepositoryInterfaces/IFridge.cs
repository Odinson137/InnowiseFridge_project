using InnowiseFridge_project.DTO;

namespace InnowiseFridge_project.Interfaces.RepositoryInterfaces;

public interface IFridge
{
    Task<List<FridgeDto>> GetFridgeAsync();
}