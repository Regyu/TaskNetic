﻿
namespace TaskNetic.Client.Models
{
    public class Label
    {
     public int Id { get; set; }
     public required string LabelName { get; set; }
     public string? comment { get; set; }
     public ColorModel? Color { get; set; }
         
    }
}
