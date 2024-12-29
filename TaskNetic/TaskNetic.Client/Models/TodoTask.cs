using System.Text.Json.Serialization;

namespace TaskNetic.Client.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public required string TaskName { get; set; }
        public bool TaskFinished { get; set; }
        [JsonIgnore]
        public CardModel? Card { get; set; }
    }
}
