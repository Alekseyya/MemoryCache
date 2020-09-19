using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryCache.Core.Models;
using MemoryCache.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MemoryCache.MainApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Route("GetUser")]
        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
            User user = await _userService.GetUser(id);
            if (user != null)
                return Content($"User: {user.Name}");
            return Content("User not found");
        }

        [Route("AddUser")]
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            if (user != null)
            {
                await _userService.AddUser(user);
                return Content($"User create {user.Name}");
            }
            return Content("User not create");
            
        }

        [Route("AddUsers")]
        [HttpPost]
        public async Task<IActionResult> AddUsers(int id, List<User> users)
        {
            if (users != null)
            {
                var outputUsers = await _userService.SetUsersFromCacheId(id, users);
                return Ok(outputUsers);
            }
            return Content("User not create");

        }

        [Route("GetUsers")]
        [HttpGet]
        public async Task<IActionResult> GetUsers(int id)
        {
            if (id != 0)
            {
                var outputUsers = await _userService.GetUsersFromCacheId(id);
                return Ok(outputUsers);
            }
            return Content("User not create");

        }
    }
}
