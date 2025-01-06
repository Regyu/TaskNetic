namespace TaskNetic.Models
{
    public class Project
    {
        public int Id { get; set; }
        public required string ProjectName { get; set; }
        public string? BackgroundImageId { get; set; }

        public ICollection<ProjectRole> ProjectRoles { get; set; } = new List<ProjectRole>();

        public ICollection<Board> ProjectBoards { get; set; } = new List<Board>();

    }
}
