using Microsoft.AspNetCore.Components;
using TaskNeticDemo.Models;
using TaskNeticDemo.Services.Interfaces;

namespace TaskNetic.Client.Services.Implementations
{
    public class ProjectService: IProjectService
    {
        private List<Project> projects = new List<Project>()
        {

        };
        public List<ProjectRole> GetProjectRoles(Project project)
        {
            
        }
    public async Task UpdateAsync(Project project)
        {
            var projectToUpdate = projects.FirstOrDefault(p => p.Id == project.Id);
            if (projectToUpdate != null)
            {
                projectToUpdate.ProjectName = project.ProjectName;
                projectToUpdate.BackgroundImageId = project.BackgroundImageId;
                projectToUpdate.ProjectRoles = project.ProjectRoles;
                projectToUpdate.ProjectBoards = project.ProjectBoards;
            }
        }
    }
}
