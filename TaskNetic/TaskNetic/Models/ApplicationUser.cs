using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace TaskNetic.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ImagePath { get; set; }
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Card> Cards { get; set; } = new List<Card>();
        public ICollection<Board> Boards { get; set; } = new List<Board>();
    }

}
