namespace ECommerceOrders.Client.Models
{
    public class CreateOrder
    {
        public List<CreateOrderItem> Products { get; set; } = [];
    }
}
