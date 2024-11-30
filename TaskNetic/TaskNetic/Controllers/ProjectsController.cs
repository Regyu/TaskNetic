using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IRepository<Project> _projectRepository;

        public ProjectsController(IProjectService projectService, IRepository<Project> projectRepository)
        {
            _projectService = projectService;
            _projectRepository = projectRepository;
        }

        // GET: api/projects/{projectId}/roles
        [HttpGet("{projectId}/roles")]
        public async Task<IActionResult> GetProjectRoles(int projectId)
        {
            try
            {
                var project = await _projectRepository.GetByIdAsync(projectId);
                if (project == null)
                    return NotFound(new { message = $"Project with ID {projectId} not found." });

                var roles = await _projectService.GetProjectRoles(project);
                return Ok(roles);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/projects/{projectId}/background
        [HttpGet("{projectId}/background")]
        public async Task<IActionResult> GetProjectBackgroundId(int projectId)
        {
            try
            {
                var project = await _projectRepository.GetByIdAsync(projectId);
                if (project == null)
                    return NotFound(new { message = $"Project with ID {projectId} not found." });

                var backgroundId = await _projectService.GetProjectBackgroundId(project);
                return Ok(new { backgroundId });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/projects/{projectId}
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProjectAndUsers(int projectId)
        {
            try
            {
                var project = await _projectRepository.GetByIdAsync(projectId);
                if (project == null)
                    return NotFound(new { message = $"Project with ID {projectId} not found." });

                await _projectService.DeleteProjectAndUsersAsync(project);
                return Ok(new { message = "Project and associated users deleted successfully." });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
