using System.Collections.Generic;


namespace TaskNetic.Client.Models
{
    public class ApplicationUser
    {
        public string? ImagePath { get; set; }
        public ICollection<CardModel> Cards { get; set; } = new List<CardModel>();
        public ICollection<ProjectRole> projectRoles { get; set; } = new List<ProjectRole>();
    }

}
