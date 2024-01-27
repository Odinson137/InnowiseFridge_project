using InnowiseFridge_project.Data;

namespace InnowiseFridge_project.Interfaces.ServiceInterfaces;

public interface ITokenService
{
    string GenerateJwtToken(string userId, string userName, Role role);
}