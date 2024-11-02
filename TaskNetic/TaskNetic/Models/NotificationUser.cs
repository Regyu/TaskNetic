namespace TaskNetic.Models
{
    public class NotificationUser
    {
    public int Id { get; set; }
    public ApplicationUser User { get; set; } = null!;
    }
}
