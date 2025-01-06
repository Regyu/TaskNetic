using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace TaskNetic.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ImagePath { get; set; }
        [JsonIgnore]
        public ICollection<Card> Cards { get; set; } = new List<Card>();
        public ICollection<ProjectRole> projectRoles { get; set; } = new List<ProjectRole>();
    }

}
