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

        //public async Task<List<Product>> GetProductsAsync(int page = 1, int size = 10, string? search = null)
        //{
        //    //var response = await _httpClient.GetAsync("api/products");
        //    //response.EnsureSuccessStatusCode();
        //    //var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        //    //return products ?? new List<Product>();

        //    var query = $"api/products?page={page}&size={size}";
        //    if (!string.IsNullOrWhiteSpace(search))
        //        query += $"&search={System.Net.WebUtility.UrlEncode(search)}";

        //    var products = await _httpClient
        //        .GetFromJsonAsync<List<Product>>(query);

        //    return products ?? new List<Product>();
        //}

        public async Task<PagedResult<Product>> GetProductsAsync(string? search = null, int page = 1, int size = 10, CancellationToken ct = default)
        {
            try
            {
                var url = $"api/products/search?page={page}&size={size}";

                if (!string.IsNullOrWhiteSpace(search))
                {
                    url += $"&q={Uri.EscapeDataString(search)}";
                }

                var result = await _httpClient.GetFromJsonAsync<PagedResult<Product>>(url, ct);
                return result ?? new PagedResult<Product> { PageNumber = page, PageSize = size };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Błąd przy pobieraniu produktów: {ex.Message}");
                return new PagedResult<Product> { PageNumber = page, PageSize = size };
            }
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
