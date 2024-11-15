namespace TaskNetic.Models
{
    public class ProjectRole
    {
      public int Id { get; set; }
      public ApplicationUser ApplicationUser { get; set; } = null!;
      public ApplicationRole ApplicationRole { get; set; } = null!;
    }
}
