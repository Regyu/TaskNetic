namespace TaskNetic.Client.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public required string TaskName { get; set; }
        public bool TaskFinished { get; set; }
        public required CardModel Card { get; set; }
    }
}
