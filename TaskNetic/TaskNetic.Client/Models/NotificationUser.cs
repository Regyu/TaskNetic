﻿namespace TaskNetic.Client.Models
{
    public class NotificationUser
    {
    public int Id { get; set; }
    public required ApplicationUser User { get; set; }
    }
}
