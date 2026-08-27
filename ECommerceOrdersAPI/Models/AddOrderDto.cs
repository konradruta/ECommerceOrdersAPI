namespace ECommerceOrdersAPI.Models
{
    public class AddOrderDto
    {
        public List<OrderProductDtoWithoutName> Products { get; set; }
    }
}
