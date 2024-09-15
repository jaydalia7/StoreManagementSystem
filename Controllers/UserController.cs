using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagementSystem.Services;
using StoreManagementSystem.Models.ViewModels;
using StoreManagementSystem.Services.IServices;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Net;
using System.Net.Mime;

namespace StoreManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDisplayModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type =typeof(Nullable))]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{ControllerName}{APIName}", nameof(UserController), nameof(GetAllUsers)))
            {
                var result = await _userService.GetAllUsers(cancellationToken);
                return result.Item1 is not NullUserListDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }

        [HttpGet("ActiveUsers")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDisplayModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> GetActiveUsers(CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{ControllerName}{APIName}", nameof(UserController), nameof(GetActiveUsers)))
            {
                var result = await _userService.GetActiveUsers(cancellationToken);
                return result.Item1 is not NullUserListDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDisplayModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> CreateUser(UserAddModel userAddModel, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{ControllerName}{APIName}", nameof(UserController), nameof(CreateUser)))
            {
                var result = await _userService.CreateUser(userAddModel, cancellationToken);
                return result.Item1 is not NullUserDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(StoreManagmentError))]

        public async Task<IActionResult> Login(UserLoginModel userLogin, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{ControllerName}{APIName}", nameof(UserController), nameof(Login)))
            {
                var result = await _userService.Login(userLogin, cancellationToken);
                return !string.IsNullOrEmpty(result.Item1) ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type =typeof(Nullable))]
        public async Task<IActionResult> RemoveUser([Required] int userId, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{ControllerName}{APIName}", nameof(UserController), nameof(RemoveUser)))
            {
                var result = await _userService.RemoveUser(userId, cancellationToken);
                return !string.IsNullOrEmpty(result.Item1) ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }
    }
}
