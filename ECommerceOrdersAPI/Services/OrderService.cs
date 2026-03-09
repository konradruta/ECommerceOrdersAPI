using AutoMapper;
using ECommerceOrdersAPI.Entities;
using ECommerceOrdersAPI.Exceptions;
using ECommerceOrdersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrdersAPI.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrders();
        Task<OrderDto> GetOrderById(int id);
        Task<int> CreateOrder(AddOrderDto dto);
        Task<bool> EditOrder(int id, EditOrderDto dto);
        Task<bool> DeleteOrder(int id);
    }
    public class OrderService: IOrderService
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly IMapper _mapper;
        public OrderService(ECommerceDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetOrders()
        {
            var orders = await _dbContext.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ToListAsync();

            var ordersMap = _mapper.Map<List<OrderDto>>(orders);

            return ordersMap;
        }

        public async Task<OrderDto> GetOrderById(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                throw new NotFoundException("Order is not found");
            }

            var orderMap = _mapper.Map<OrderDto>(order);

            return orderMap;
        }

        public async Task<int> CreateOrder(AddOrderDto dto)
        {
            var productIds = dto.Products.Select(p => p.ProductId).ToList();

            var products = await _dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            var totalAmount = dto.Products.Sum(p =>
            {
                var product = products.FirstOrDefault(x => x.Id == p.ProductId);

                if (product == null)
                {
                    throw new NotFoundException($"Product {p.ProductId} not found");
                }
                return product.Price * p.Quantity;
            });

            var newOrder = new Order() {
                CreatedAt = DateTime.UtcNow,
                TotalAmount = totalAmount,
                OrderProducts = dto.Products.Select(p => new OrderProduct
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                })
                .ToList()
            };

            _dbContext.Orders.Add(newOrder);
            await _dbContext.SaveChangesAsync();

            return newOrder.Id;
        }

        public async Task<bool> EditOrder(int id, EditOrderDto dto)
        {
            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                throw new NotFoundException("Order is not found");
            }

            _mapper.Map(dto, order);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                throw new NotFoundException("Order is not found");
            }

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
