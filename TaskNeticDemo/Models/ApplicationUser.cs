using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TaskNeticDemo.Models
{
    public class ApplicationUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? ImagePath { get; set; }
        [JsonIgnore]
        public ICollection<CardModel> Cards { get; set; } = new List<CardModel>();
        public ICollection<ProjectRole> projectRoles { get; set; } = new List<ProjectRole>();
    }

}
