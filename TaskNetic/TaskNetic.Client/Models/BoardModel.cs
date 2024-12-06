using System.Reflection.Emit;

namespace TaskNetic.Client.Models
{
    public class BoardModel
    {
        public int BoardId { get; set; }
        public required string Title { get; set; }

        public ICollection<List> Lists { get; set; } = new List<List>();

        public ICollection<Label> Labels { get; set; } = new List<Label>();

    }
}
