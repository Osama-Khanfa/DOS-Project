namespace CatalogServer.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Stock { get; set; }
        public int Price { get; set; }
        public string Category { get; set; } = Topics.distributed_systems.ToString();
    }
}
