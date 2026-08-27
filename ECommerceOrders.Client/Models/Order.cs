namespace ECommerceOrders.Client.Models
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        public List<OrderProduct> OrderProducts { get; set; } = [];
    }
}
