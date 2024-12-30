using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Squib.UserService.API.DB;
using Squib.UserService.API.Model;
using Squib.UserService.API.Repository;

namespace Squib.UserService.API;

public class UserRepo : IUserRepo
{
    private readonly ILogger<UserRepo> _logger;
    private readonly ApplicationDbContext _context;

    public UserRepo(ILogger<UserRepo> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<UserDto>> GetUsers()
    {
        try
        {
            var users = await _context.UserDtos.ToListAsync();
            _logger.LogInformation($"Total users fetched: {users.Count}");
            return users;
        }
        catch (Exception e)
        {
            _logger.LogError($"{e.Message}\n{e.StackTrace}");
            return new List<UserDto>();
        }
    }

    public async Task<UserDto> GetUserById(int id)
    {
        try
        {
            var user = await _context.UserDtos
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found");
            }

            return user;
        }
        catch (Exception e)
        {
            _logger.LogError($"{e.Message}\n{e.StackTrace}");
            return null;
        }
    }

    public async Task<bool> AddUser(UserDto user)
    {
        try
        {
            await _context.UserDtos.AddAsync(user);
            var result = await _context.SaveChangesAsync();
            _logger.LogInformation($"User added successfully: {user.Email}");
            return result > 0;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError($"Database error while adding user: {e.Message}");
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError($"Error adding user: {e.Message}\n{e.StackTrace}");
            return false;
        }
    }

    public async Task<bool> UpdateUser(UserDto user)
    {
        try
        {
            var existingUser = await _context.UserDtos.FindAsync(user.Id);
            if (existingUser == null)
            {
                _logger.LogWarning($"User with ID {user.Id} not found for update");
                return false;
            }

            existingUser.Email = user.Email;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;

            _context.UserDtos.Update(existingUser);
            var result = await _context.SaveChangesAsync();
            _logger.LogInformation($"User updated successfully: {user.Email}");
            return result > 0;
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogError($"Concurrency error while updating user: {e.Message}");
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError($"Error updating user: {e.Message}\n{e.StackTrace}");
            return false;
        }
    }

    public async Task<bool> DeleteUser(int id)
    {
        try
        {
            var user = await _context.UserDtos.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found for deletion");
                return false;
            }

            _context.UserDtos.Remove(user);
            var result = await _context.SaveChangesAsync();
            _logger.LogInformation($"User deleted successfully: ID {id}");
            return result > 0;
        }
        catch (Exception e)
        {
            _logger.LogError($"Error deleting user: {e.Message}\n{e.StackTrace}");
            return false;
        }
    }
}