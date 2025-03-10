﻿using System.Net.Mail;

namespace TaskNetic.Models
{
    public class Card
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
        public ICollection<Label> CardLabels { get; set; } = new List<Label>();
        public ICollection<ApplicationUser> CardMembers { get; set; } = new List<ApplicationUser>();
    }
}
