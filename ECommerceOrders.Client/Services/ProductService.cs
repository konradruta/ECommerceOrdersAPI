using ECommerceOrders.Client.Models;
using System.Net.Http.Json;

namespace ECommerceOrders.Client.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            //var response = await _httpClient.GetAsync("api/products");
            //response.EnsureSuccessStatusCode();
            //var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            //return products ?? new List<Product>();

            var products = await _httpClient
            .GetFromJsonAsync<List<Product>>("api/products");

            return products ?? [];
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/products/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditProduct(Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/products/{product.Id}", product);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddProduct(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("api/products", product); ;

            return response.IsSuccessStatusCode;
        }
    }
}
