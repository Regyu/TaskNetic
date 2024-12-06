namespace TaskNetic.Client.Models
{
    public class List
    {
     public int Id { get; set; }
     public required string Title { get; set; }
     public ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
