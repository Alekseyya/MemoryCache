using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryCache.Core.Models;

namespace MemoryCache.Core.Services
{
    public interface IUserService
    {
       void Initialize();
       Task<IEnumerable<User>> GetUsers();
       Task AddUser(User user);
       Task UpdateUser(User user);
       Task<User> GetUser(int id);
       Task<List<User>> SetUsersFromCacheId(int id, List<User> users);
       Task<List<User>> GetUsersFromCacheId(int id);
       Task<List<User>> GetUsersFromRedis(string id);
       Task<List<User>> SetUsersFromRedis(string id, List<User> inputUsers);
    }
}
