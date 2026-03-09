using AutoMapper;
using ECommerceOrdersAPI.Entities;
using ECommerceOrdersAPI.Exceptions;
using ECommerceOrdersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrdersAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProductById(int id);
        Task<int> AddProduct(AddProductDto dto);
        Task<bool> EditProduct(int id, EditProductDto dto);
        Task<bool> DeleteProduct(int id);
    }
    public class ProductService : IProductService
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly IMapper _mapper;
        public ProductService(ECommerceDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await _dbContext.Products
                .ToListAsync();

            var productsDto = _mapper.Map<List<ProductDto>>(products);

            return productsDto;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Product Not Found");
            }

            var productMap = _mapper.Map<ProductDto>(product);

            return productMap;
        }

        public async Task<int> AddProduct(AddProductDto dto)
        {
            var newProduct = _mapper.Map<Product>(dto);

            _dbContext.Products.Add(newProduct);
            await _dbContext.SaveChangesAsync();

            return newProduct.Id;
        }

        public async Task<bool> EditProduct(int id, EditProductDto dto)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Product Not Found");
            }

            _mapper.Map(dto, product);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _dbContext.Products
                .Include(p => p.OrderProducts)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Product Not Found");
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
