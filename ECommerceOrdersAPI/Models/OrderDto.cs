using ECommerceOrdersAPI.Entities;

namespace ECommerceOrdersAPI.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "New";
        public DateTime CreatedAt { get; set; }

        public List<OrderProductDto> OrderProducts { get; set; }
    }
}
