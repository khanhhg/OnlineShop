using AutoMapper;
using WebBanHangOnline.Data.Models.Dtos;
using WebBanHangOnline.Data.Models.EF;

namespace WebBanHangOnline.Data
{
    public class AppMapperProfile: Profile
    {     
        public AppMapperProfile()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
        }
    }  
}
