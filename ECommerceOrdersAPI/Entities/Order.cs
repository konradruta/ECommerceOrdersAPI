namespace ECommerceOrdersAPI.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalAmount {  get; set; }
        public string Status { get; set; } = "New";
        public DateTime CreatedAt { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
