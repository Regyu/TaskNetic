using System.Net.Mail;

namespace TaskNetic.Client.Models
{
    public class CardModel
    {
        public int CardId { get; set; }
        public required string CardTitle { get; set; }
        public string? CardDescription { get; set; }
        public int? CardPosition { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public DateTime? DueDate { get; set; }
        public ICollection<FileAttachment> Attachments { get; set; } = new List<FileAttachment>();
        public TaskList? TaskList { get; set; }
        public ICollection<Label> CardLabels { get; set; } = new List<Label>();
        public ICollection<ApplicationUser> CardMembers { get; set; } = new List<ApplicationUser>();
    }
}
