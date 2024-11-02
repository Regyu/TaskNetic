namespace TaskNetic.Models
{
    public class Project
    {
        public int Id { get; set; }
        public String? ProjectName { get; set; }

        public ICollection<ApplicationUser> ProjectUsers = new List<ApplicationUser>();

        public ICollection<ProjectRole> ProjectRoles = new List<ProjectRole>();

        public ICollection<Board> ProjectBoards = new List<Board>();

    }
}
