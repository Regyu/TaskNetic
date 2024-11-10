using System.Reflection.Emit;

namespace TaskNetic.Models
{
    public class Board
    {
        public int BoardId { get; set; }
        public required string Title { get; set; }

        public ICollection<List> Lists { get; set; } = new List<List>();

        public ICollection<ApplicationUser> BoardUsers { get; set; } = new List<ApplicationUser>();

        public ICollection<Label> Labels { get; set; } = new List<Label>();

        public int BackgroundId { get; set; }



    }
}
