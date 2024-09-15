using AutoMapper;
using StoreManagementSystem.Models.Domains;
using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Mappings
{
    public class StoreManagementMapping : Profile
    {
        public StoreManagementMapping()
        {
            CreateMap<User, UserDisplayModel>();
            CreateMap<Product, ProductDisplayModel>();
            CreateMap<PurchaseProduct, PurchaseProductDisplayModel>();
            CreateMap<PurchaseProductAddModel, PurchaseProduct>()
                .ForMember(p => p.ProductId, dest => dest.MapFrom(scr => scr.ProductId))
                .ForMember(p => p.Quantity, dest => dest.MapFrom(scr => scr.Quantity));
            CreateMap<SellProduct, SellProductDisplayModel>();
            CreateMap<SellProductAddModel, SellProduct>()
                .ForMember(sp => sp.ProductId, dest => dest.MapFrom(scr => scr.ProductId))
                .ForMember(sp => sp.Quantity, dest => dest.MapFrom(scr => scr.Quantity));
            CreateMap<Product, StockProductDisplayModel>()
                .ForMember(sp => sp.ProductName, dest => dest.MapFrom(scr => scr.Name))
                .ForMember(sp => sp.ProductDescription, dest => dest.MapFrom(scr => scr.Name));

        }
    }
}
