namespace OrderServer.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public DateOnly date { get; set; }
    }
}
