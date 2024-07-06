using AutoMapper;
using E_Commerce.API.Dtos.Basket;
using E_Commerce.API.Dtos.Identity;
using E_Commerce.API.Dtos.Order;
using E_Commerce.API.Dtos.Product;
using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Entities.Order_Aggregate;
using E_Commerce.Core.Entities.Product;

namespace E_Commerce.API.Helpers
{
    public class MappingProfiles:Profile
	{
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.Brand,O=>O.MapFrom(S=>S.Brand.Name))
                .ForMember(d=>d.Category,O=>O.MapFrom(S=>S.Category.Name))
                .ForMember(d=>d.PictureUrl,O=>O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>(); 
            CreateMap<Address,AddressDto>().ReverseMap();

            CreateMap<OrderAddressDto, OrderAddress>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<OrderItemPictureUrlResolver>());

        }
    }
}
