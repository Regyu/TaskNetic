namespace TaskNetic.Models
{
    public class BoardPermission
    {
        public int Id { get; set; }
        public required ProjectRole Role { get; set; }
        public required Board Board { get; set; }
        public bool CanEdit { get; set; }
    }
}
