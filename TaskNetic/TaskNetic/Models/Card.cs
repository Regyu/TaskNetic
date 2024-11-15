using System.Net.Mail;

namespace TaskNetic.Models
{
    public class Card
    {
        public int CardId { get; set; }
        public string? CardTitle { get; set; }
        public required string CardDescription { get; set; }
         public DateTime CreatedAt { get; set; }
         public ICollection<Comment> Comments { get; set; } =new List<Comment>();
         public DateTime DueDate { get; set; }
         public ICollection<FileAttachment> Attachments { get; set; } = new List<FileAttachment>();
         public TaskList TaskList { get; set; } = null!;
         public ICollection<Label> CardLabels { get; set; } = new List<Label>();
         public ICollection<ApplicationUser> CardMembers { get; set; } = new List<ApplicationUser>();
         public ICollection<NotificationUser> NotificationUsers { get; set; } = new List<NotificationUser>();
    }
}
