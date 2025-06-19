namespace TaskNeticDemo.Models
{
    public class ProjectRole
    {
        public int Id { get; set; }
        public required ApplicationUser ApplicationUser { get; set; }
        public required Project Project { get; set; }
        public ICollection<BoardPermission> BoardPermissions { get; set; } = new List<BoardPermission>();
        public bool isAdmin { get; set; }
    }
}
