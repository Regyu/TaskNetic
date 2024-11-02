namespace TaskNetic.Models
{
    public class FileAttachment
    {
    public int Id { get; set; }
    public required string FileName { get; set; }
    public required string FilePath { get; set; }
    public DateTime UploadedAt { get; set; }
    public int UploadedUserId { get; set; }
    }
}
