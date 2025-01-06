using System.Text.Json.Serialization;

namespace TaskNetic.Models
{
    public class Board
    {
        public int BoardId { get; set; }
        public required string Title { get; set; }

        public ICollection<List> Lists { get; set; } = new List<List>();
        [JsonIgnore]
        public ICollection<BoardPermission> BoardPermissions { get; set; } = new List<BoardPermission>();

        public ICollection<Label> Labels { get; set; } = new List<Label>();

    }
}
