namespace TheWag.Api.WagDB
{
    using AutoMapper;
    using TheWag.Functions.EF;
    using TheWag.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
            CreateMap<Tag, TagDTO>();
            CreateMap<TagDTO, Tag>();

            CreateMap<CustomerCart, Order>();
        }
    }
}
