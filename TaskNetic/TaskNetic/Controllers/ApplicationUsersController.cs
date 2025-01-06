using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;
using TaskNetic.Client.DTO;
using TaskNetic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using TaskNetic.Services.Implementations;

namespace TaskNetic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationUsersController : ControllerBase
    {

        private readonly IApplicationUserService _applicationUserService;

        public ApplicationUsersController(IApplicationUserService applicationUserService)
        {
           _applicationUserService = applicationUserService;
        }

        [HttpGet("get-user-id/{userName}")]
        public async Task<IActionResult> GetIdByUserNameAsync(string userName)
        {
            try
            {
                ApplicationUser user = await _applicationUserService.GetByUserNameAsync(userName);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
