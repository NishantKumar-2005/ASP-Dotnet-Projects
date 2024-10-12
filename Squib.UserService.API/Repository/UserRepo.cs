using System.Data;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Squib.UserService.API.config;
using Squib.UserService.API.model;
using Squib.UserService.API.Model;
using Squib.UserService.API.Repository;
namespace Squib.UserService.API;

public class UserRepo : IUserRepo
{
    private readonly ILogger<UserRepo> _logger;
    private readonly ConnectionString _connectionString;

    

    public UserRepo(ILogger<UserRepo> logger, IOptions<ConnectionString> connectionStringOption )
    {
        _logger = logger;
        _connectionString = connectionStringOption.Value;
        
    }
public async Task<List<UserDto>> GetUsers()
{
    List<UserDto> userData = new List<UserDto>();
    
    try
    {
        using var connection = new SqlConnection(_connectionString.MyDb);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "sp_GetUsers";
        
        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            userData.Add(new UserDto
            {
                Id = reader.GetInt32(0),
                Email = reader.GetString(1),
                FirstName = reader.GetString(2),
                LastName = reader.GetString(3)
            });
        }
        _logger.LogInformation($"Total users fetched: {userData.Count}");
    }
    catch (Exception e)
    {
        _logger.LogError($"{e.Message}\n{e.StackTrace}");
    }

    return userData;
}




public UserDto GetUserById(int id)
{
    UserDto UserData = null;
    try
    {
        using var connection = new SqlConnection(_connectionString.MyDb);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "sp_GetUserById";
        command.Parameters.AddWithValue("@Id", id);
        
        connection.Open();
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            UserData = new UserDto
            {
                Id = reader.GetInt32(0),
                Email = reader.GetString(1),
                FirstName = reader.GetString(2),
                LastName = reader.GetString(3)
            };
        }
    }
    catch (Exception e)
    {
        _logger.LogError($"{e.Message}\n{e.StackTrace}");
    }
    return UserData;
}

public async Task<bool> AddUser(UserDto user)
{
    try
    {
        using var connection = new SqlConnection(_connectionString.MyDb);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "sp_add_custom_user";

        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@FirstName", user.FirstName);
        command.Parameters.AddWithValue("@LastName", user.LastName);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        return true;
    }
    catch (Exception e)
    {
        _logger.LogError($"{e.Message}\n{e.StackTrace}");
        return false;
    }
}



 public bool UpdateUser(UserDto user)
{
    try
    {
        using var connection = new SqlConnection(_connectionString.MyDb);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "sp_UpdateUser";

        command.Parameters.AddWithValue("@Id", user.Id);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@FirstName", user.FirstName);
        command.Parameters.AddWithValue("@LastName", user.LastName);

        connection.Open();
        int rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }
    catch (Exception e)
    {
        _logger.LogError($"{e.Message}\n{e.StackTrace}");
        return false;
    }
}



public bool DeleteUser(int id)
{
    try
    {
        using var connection = new SqlConnection(_connectionString.MyDb);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "sp_DeleteUser";

        command.Parameters.AddWithValue("@Id", id);

        connection.Open();
        command.ExecuteNonQuery();
        return true;
    }
    catch (Exception e)
    {
        _logger.LogError($"{e.Message}\n{e.StackTrace}");
        return false;
    }
}


    
}
