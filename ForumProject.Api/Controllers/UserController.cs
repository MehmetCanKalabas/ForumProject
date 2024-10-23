using ForumProjects.Application.Services;
using ForumProjects.Infrastructure.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult UserGetAll()
        {
            var userList = _userService.GetAll();
            return Ok(userList);
        }

        [HttpGet("{Id}")]
        public IActionResult UserGetById(string Id)
        {
            try
            {
                var user = _userService.GetById(Id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Kullanıcı bulunamadı"))
                {
                    return NotFound(ex.Message);
                }

                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> UserCreate(AccountCreateDTO model)
        {
            var result = await _userService.UserCreate(model);

            if (result.Success)
            {
                return CreatedAtAction(nameof(UserCreate), new { id = model.Email }, result);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UserUpdate(AccountCreateDTO model)
        {
            var result = await _userService.UserUpdate(model);

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

    }
}
