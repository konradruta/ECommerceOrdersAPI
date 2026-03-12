using AutoMapper;
using ECommerceOrdersAPI.Entities;
using ECommerceOrdersAPI.Models;

namespace ECommerceOrdersAPI
{
    public class ECommerceMappingProfile : Profile
    {
        public ECommerceMappingProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderProducts));

            CreateMap<OrderProduct, OrderProductDto>();

            CreateMap<AddOrderDto, Order>();

            CreateMap<EditOrderDto, Order>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProductDto, Product>();

            CreateMap<Product, ProductDto>();

            CreateMap<AddProductDto, Product>();

            CreateMap<EditProductDto, Product>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));


        }
    }
}
