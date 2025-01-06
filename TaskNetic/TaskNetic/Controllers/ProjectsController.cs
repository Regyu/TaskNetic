using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;
using TaskNetic.Client.DTO;

namespace TaskNetic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                Project? project = await _projectRepository.GetByIdAsync(projectId);
                if (project == null)
                    return NotFound(new { message = $"Project with ID {projectId} not found." });

                IEnumerable<ProjectRole> roles = await _projectService.GetProjectRoles(project);
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

        // GET: api/projects/{projectId}/info
        [HttpGet("{projectId}/info")]
        public async Task<IActionResult> GetProjectInfo(int projectId)
        {
            try
            {
                Project? project = await _projectRepository.GetByIdAsync(projectId);
                if (project == null)
                    return NotFound(new { message = $"Project with ID {projectId} not found." });

                return Ok(new ProjectInfo { ProjectId = project.Id, Name = project.ProjectName, BackgroundId = project.BackgroundImageId });
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
                Project? project = await _projectRepository.GetByIdAsync(projectId);
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
