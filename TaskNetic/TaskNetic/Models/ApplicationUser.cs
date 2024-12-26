using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace TaskNetic.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ImagePath { get; set; }
        public ICollection<ProjectRole> projectRoles { get; set; } = new List<ProjectRole>();
    }

}
