using MemoryCache.Core.Models;
using MemoryCache.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryCache.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCache.BL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IMemoryCache _memoryCache;
        public UserService(ApplicationContext applicationContext, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _applicationContext = applicationContext;
        }
        public void Initialize()
        {
            _applicationContext.Users.AddRange(
                new User { Name = "Tom", Email = "tom@gmail.com", Age = 35 },
                new User { Name = "Alice", Email = "alice@yahoo.com", Age = 29 },
                new User { Name = "Sam", Email = "sam@online.com", Age = 37 }
            );
            _applicationContext.SaveChanges();
        }
        public async Task AddUser(User user)
        {
            _applicationContext.Users.Add(user);
            int n = await _applicationContext.SaveChangesAsync();
            if (n > 0)
            {
                _memoryCache.Set(user.Id, user, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                });
            }
        }
        public async Task<User> GetUser(int id)
        {
            User user = null;
            if (!_memoryCache.TryGetValue(id, out user))
            {
                user = await _applicationContext.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    _memoryCache.Set(user.Id, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
                }
            }
            return user;
        }

        public async Task<List<User>> SetUsersFromCacheId(int id, List<User> inputUsers)
        {
            if (!_memoryCache.TryGetValue(id, out List<User> users))
            {
                users = await _applicationContext.Users.ToListAsync();
                if(inputUsers != null)
                    users.AddRange(inputUsers);
                _memoryCache.Set(id, users, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(2)
                });
            }
            return users;
        }

        public Task<List<User>> GetUsersFromCacheId(int id)
        {
            if (!_memoryCache.TryGetValue(id, out List<User> users))
            {
                return Task.FromResult(users);
            }
            return Task.FromResult(users);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _applicationContext.Users.ToListAsync();
        }

        public Task UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
