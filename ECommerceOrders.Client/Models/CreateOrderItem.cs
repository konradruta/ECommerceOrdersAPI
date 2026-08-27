namespace ECommerceOrders.Client.Models
{
    public class CreateOrderItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }

    public class SelectedOrderProduct
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = "";

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
