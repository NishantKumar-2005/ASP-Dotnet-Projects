using Squib.UserService.API.model;
using Squib.UserService.API.Model;

namespace Squib.UserService.API.Repository;

public interface IUserRepo
{
    public Task<List<UserDto>> GetUsers();
    public Task<UserDto> GetUserById(int id);

    public Task<bool> AddUser(UserDto user);

    public Task<bool> UpdateUser(UserDto user);

    public Task<bool> DeleteUser(int id);

   



}
