namespace TaskNetic.Client.Models
{
    public class BoardPermission
    {
        public int Id { get; set; }
        public required ProjectRole Role { get; set; }
        public required BoardModel Board { get; set; }
        public bool CanEdit { get; set; }
    }
}
