using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;


namespace TaskNetic.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public String? ImagePath;
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Card> Cards { get; set; } = new List<Card>();
        public ICollection<Board> Boards { get; set; } = new List<Board>();
    }

}
