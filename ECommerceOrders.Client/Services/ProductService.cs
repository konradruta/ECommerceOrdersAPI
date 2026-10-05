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

        public async Task<List<Product>> GetProductsAsync(int page = 1, int size = 10, string? search = null)
        {
            //var response = await _httpClient.GetAsync("api/products");
            //response.EnsureSuccessStatusCode();
            //var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            //return products ?? new List<Product>();

            var query = $"api/products?page={page}&size={size}";
            if (!string.IsNullOrWhiteSpace(search))
                query += $"&search={System.Net.WebUtility.UrlEncode(search)}";

            var products = await _httpClient
                .GetFromJsonAsync<List<Product>>(query);

            return products ?? new List<Product>();
        }

        public async Task<int> GetProductsCountAsync(string? search = null)
        {
            var query = "api/products/count";

            if (!string.IsNullOrWhiteSpace(search))
            {
                query += $"?search={System.Net.WebUtility.UrlEncode(search)}";
            }

            var totalCount = await _httpClient.GetFromJsonAsync<int?>(query);
            return totalCount ?? 0;
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
