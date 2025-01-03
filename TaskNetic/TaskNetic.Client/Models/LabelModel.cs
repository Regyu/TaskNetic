namespace TaskNetic.Client.Models
{
    public class LabelModel
    {
        public int Id { get; set; }
        public required string LabelName { get; set; }
        public string? Comment { get; set; }
        public required string ColorCode { get; set; }

    }
}
