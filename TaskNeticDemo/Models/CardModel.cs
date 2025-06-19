using System.Net.Mail;

namespace TaskNeticDemo.Models
{
    public class CardModel
    {
        public int CardId { get; set; }
        public int CardPosition { get; set; }
        public required string CardTitle { get; set; }
        public string? CardDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public DateTime? DueDate { get; set; }
        public ICollection<FileAttachment> Attachments { get; set; } = new List<FileAttachment>();
        public ICollection<TodoTask> TodoTasks { get; set; } = new List<TodoTask>();
        public ICollection<LabelModel> CardLabels { get; set; } = new List<LabelModel>();
        public ICollection<ApplicationUser> CardMembers { get; set; } = new List<ApplicationUser>();
    }
}
