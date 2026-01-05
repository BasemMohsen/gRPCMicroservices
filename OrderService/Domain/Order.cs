namespace OrderService.Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; } = default!;
        public double Amount { get; set; }
        public string Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
