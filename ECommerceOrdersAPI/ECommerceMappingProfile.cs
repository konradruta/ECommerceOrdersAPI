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

            CreateMap<EditOrderDto, Order>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));
            /*.ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status != null))
            .ForMember(dest => dest.TotalAmount, opt => opt.Condition(src => src.TotalAmount != 0));*/

            CreateMap<ProductDto, Product>();

            CreateMap<Product, ProductDto>();

            CreateMap<EditProductDto, Product>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));
            /*.ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
            .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price != 0))
            .ForMember(dest => dest.StockQuantity, opt => opt.Condition(src => src.StockQuantity != 0));*/


        }
    }
}
