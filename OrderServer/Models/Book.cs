namespace OrderServer.Models
{
    public class Book
    {
        public int bookId { get; set; }
        public string title { get; set; } = string.Empty;
        public int stock { get; set; }
        public int price { get; set; }
        public string category { get; set; }
    }
}
