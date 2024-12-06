using System.Collections.Generic;


namespace TaskNetic.Client.Models
{
    public class ApplicationUser
    {
        public string? ImagePath { get; set; }
        public ICollection<Card> Cards { get; set; } = new List<Card>();
        public ICollection<ProjectRole> projectRoles { get; set; } = new List<ProjectRole>();
    }

}
