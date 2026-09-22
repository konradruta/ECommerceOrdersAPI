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
        Task<PagedResult<ProductDto>> GetAndSearchProducts(string productName, int pageNumber, int pageSize, CancellationToken cancellation = default);
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
        private const int MaxPageSize = 100;

        public async Task<PagedResult<ProductDto>> GetAndSearchProducts(string productName, int pageNumber, int pageSize, CancellationToken cancellation = default)
        {
            if (productName == null || productName.Length < 3)
            {
                throw new Exception("Search phrase must be at least 3 characters long.");
            }

            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, MaxPageSize);

            var query = _dbContext.Products
                .AsQueryable()
                .Where(p => p.Name.Contains(productName));

            var totalCount = await query.CountAsync(cancellation);

            var products = await query
                .OrderBy(p => p.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync(cancellation);

            return new PagedResult<ProductDto>
            {
                Items = _mapper.Map<List<ProductDto>>(products),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
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
