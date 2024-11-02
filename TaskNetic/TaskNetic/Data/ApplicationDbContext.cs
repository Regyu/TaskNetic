using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskNetic.Models;
using Microsoft.AspNetCore.Identity;

namespace TaskNetic.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {
        public DbSet<TestItem> TestItems { get; set; }      // OBJEKT TESTOWY, MO¯NA WYWALIÆ
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectRole> ProjectRoles { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<FileAttachment> Attachments { get; set; }
        public DbSet<TaskList> TaskLists { get; set; }
        public DbSet<TodoTask> TodoTasks { get; set; }
        public DbSet<NotificationUser> NotificationUsers { get; set; }
    }

}
