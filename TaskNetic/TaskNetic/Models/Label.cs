using System.Drawing;

namespace TaskNetic.Models
{
    public class Label
    {
     public int Id { get; set; }
     public required string LabelName { get; set; }
     public string? Comment { get; set; }
     public required string ColorCode { get; set; }
         
    }
}
