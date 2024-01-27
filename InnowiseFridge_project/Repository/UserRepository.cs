using InnowiseFridge_project.Data;
using InnowiseFridge_project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InnowiseFridge_project.Interfaces.RepositoryInterfaces;

public class UserRepository : IUser
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public Task<int> SaveAsync()
    {
        return _context.SaveChangesAsync();
    }

    public ValueTask<EntityEntry<User>> AddUserAsync(User user)
    {
        return _context.Users.AddAsync(user);
    }

    public Task<bool> UserNameCheckTakenAsync(string userName)
    {
        return _context.Users
            .Where(u => u.UserName == userName)
            .AnyAsync();
    }

    public ValueTask<User?> GetUserById(string userId)
    {
        return _context.Users.FindAsync(userId);
    }

    public Task<User?> GetUserByName(string userName)
    {
        return _context.Users.SingleOrDefaultAsync(u => u.UserName == userName);
    }

    public Task<int> ChangeRole(string userId, Role newRole)
    {
        return _context.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(e => e.SetProperty(prop => prop.Role, newRole));
    }
}