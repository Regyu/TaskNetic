using Microsoft.AspNetCore.Mvc;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectRoleController : ControllerBase
    {
        private readonly IProjectRoleService _projectRoleService;

        public ProjectRoleController(IProjectRoleService projectRoleService)
        {
            _projectRoleService = projectRoleService;
        }

        [HttpGet("is-admin/{projectId}/{userId}")]
        public async Task<IActionResult> IsUserAdmin(int projectId, string userId)
        {
            try
            {
                var isAdmin = await _projectRoleService.IsUserAdminInProjectAsync(projectId, userId);
                return Ok(isAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
