namespace TaskNetic.Client.DTO;

public class ProjectInfo
{
    public int ProjectId { get; set; }
    public required string Name { get; set; }
    public string? BackgroundId { get; set; }
}
