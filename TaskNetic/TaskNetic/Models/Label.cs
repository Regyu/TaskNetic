using System.Drawing;

namespace TaskNetic.Models
{
    public class Label
    {
     public int Id { get; set; }
     public required string LabelName { get; set; }
     public string? comment { get; set; }
     public Color? Color { get; set; }
         
    }
}
