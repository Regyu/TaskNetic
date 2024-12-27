namespace TaskNetic.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public required string TaskName { get; set; }
        public bool TaskFinished { get; set; }
        public required Card Card { get; set; }
    }
}
