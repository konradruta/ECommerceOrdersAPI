using ECommerceOrders.Client.Models;
using System.Net.Http.Json;

namespace ECommerceOrders.Client.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var orders = await _httpClient
            .GetFromJsonAsync<List<Order>>("api/orders");

            return orders ?? [];
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/orders/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditOrder(Order order)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/orders/{order.Id}", order);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddOrder(CreateOrder order)
        {
            var response = await _httpClient.PostAsJsonAsync("api/orders", order);

            return response.IsSuccessStatusCode;
        }
    }
}
