using AutoMapper;
using Farmers.App.Models;
using Farmers.DataModel;

namespace Farmers.App.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig() 
        {
            CreateMap<Courier, CreateCourierViewModel>().ReverseMap();
            CreateMap<Product, ProductVM>().ReverseMap();
            CreateMap<Farmer, FarmerVM>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailsVM>().ReverseMap();
            CreateMap<Order, OrderDetailsVM>().ReverseMap();
        }
    }
}
