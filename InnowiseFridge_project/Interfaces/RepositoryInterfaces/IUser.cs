using InnowiseFridge_project.Data;
using InnowiseFridge_project.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InnowiseFridge_project.Interfaces.RepositoryInterfaces;

public interface IUser
{
    Task<int> SaveAsync();
    ValueTask<EntityEntry<User>> AddUserAsync(User user);
    Task<bool> UserNameCheckTakenAsync(string userName);
    ValueTask<User?> GetUserById(string userId);
    Task<User?> GetUserByName(string userName);
    Task<int> ChangeRole(string userId, Role newRole);
}