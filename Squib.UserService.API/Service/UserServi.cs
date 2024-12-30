using System.Text;
using AutoMapper;
// using Microsoft.Extensions.Caching;
// using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Squib.UserService.API.model;
using Squib.UserService.API.Model;
using Squib.UserService.API.Repository;
namespace Squib.UserService.API.Service;

public class UserServi: IUSER_Service
{
    private readonly IUserRepo _userRepo;
    private readonly IMapper _mapper;

    private string key = "Master";

    // public readonly IDistributedCache _distributedCache;
    
    public UserServi(IUserRepo userRepo, IMapper mapper ) //, IDistributedCache distributedCache
    {
        _userRepo = userRepo;
        _mapper = mapper;
        // _distributedCache = distributedCache;
    }

public async Task<List<UserRDto>> GetUsers()
{
    List<UserDto> userData = new List<UserDto>();
    // byte[] encodedList = await _distributedCache.GetAsync(key);
    // // encodedList = null; // Remove this line

    // if (encodedList != null)
    // {
    //     try
    //     {
    //         userData = JsonConvert.DeserializeObject<List<UserDto>>(Encoding.UTF8.GetString(encodedList));
    //         Console.WriteLine($"Data retrieved from cache. User count: {userData.Count}");
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Cache deserialization error: {ex.Message}");
    //         await _distributedCache.RemoveAsync(key); // Handle cache corruption
    //     }
    // }

    // if (encodedList == null || userData == null || userData.Count == 0)
    // {
        userData = await _userRepo.GetUsers();
        Console.WriteLine($"Fetched {userData.Count} users from DB");

    //     if (userData != null)
    //     {
    //         try
    //         {
    //             string jsonString = JsonConvert.SerializeObject(userData);
    //             Console.WriteLine($"Serialized JSON for caching: {jsonString}"); // Output the serialized string

    //             encodedList = Encoding.UTF8.GetBytes(jsonString);

    //             var cacheEntryOptions = new DistributedCacheEntryOptions()
    //                 .SetSlidingExpiration(TimeSpan.FromMinutes(20))
    //                 .SetAbsoluteExpiration(TimeSpan.FromHours(10));

    //             await _distributedCache.SetAsync(key, encodedList, cacheEntryOptions);
    //             Console.WriteLine("Data retrieved from DB and cached");
    //         }
    //         catch (Exception ex)
    //         {
    //             Console.WriteLine($"Cache serialization error: {ex.Message}");
    //         }
    //     }
    // }

    var userRData = _mapper.Map<List<UserRDto>>(userData);
    // Console.WriteLine($"Returning {userRData.Count} users");
    
    return userRData;
}






    public Task<UserDto> GetUserById(int id)
    {
        return _userRepo.GetUserById(id);
    }

    public Task<bool> UpdateUser(UserDto user){
        return _userRepo.UpdateUser(user);
    }
    public Task<bool> DeleteUser(int id){
        return _userRepo.DeleteUser(id);
    }
public async Task<bool> AddUser(UserDto user){
    //     var serializedUser = JsonConvert.SerializeObject(user);
    //     var Encodted = Encoding.UTF8.GetBytes(serializedUser);
    //     var userKey = $"user:{user.Id}";
    //     await _distributedCache.SetAsync(key, Encodted, new DistributedCacheEntryOptions
    // {
    //     AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
    // });
    
    return await _userRepo.AddUser(user);
}

}
