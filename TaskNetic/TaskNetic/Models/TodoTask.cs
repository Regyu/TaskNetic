namespace TaskNetic.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public string? TaskName { get; set; }
        public bool TaskFinished { get; set; }
        public TaskList TaskList { get; set; } = null!;
    }
}
