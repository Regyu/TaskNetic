namespace TaskNeticDemo.Models
{
    public class List
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int Position { get; set; }
        public ICollection<CardModel> Cards { get; set; } = new List<CardModel>();
    }
}
