namespace TaskNetic.Client.Models
{
    public class TaskList
    {
       public int Id { get; set; }
       public ICollection<TodoTask> TodoTasks { get; set; } = new List<TodoTask>();
    }
}
