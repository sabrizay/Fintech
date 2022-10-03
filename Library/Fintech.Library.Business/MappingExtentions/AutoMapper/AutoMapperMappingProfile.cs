using AutoMapper;
using Fintech.Library.Entities.Concrete;

namespace Fintech.Library.Business.MappingExtentions.AutoMapper;

public class AutoMapperMappingProfile : Profile
{
    public AutoMapperMappingProfile()
    {



        //CreateMap<ProductAttributeDto, ProductAttribute>().ReverseMap();
        //CreateMap<ProductAttributeDto, ProductAttributeDesc>().ReverseMap();

        //CreateMap<ProductBasePriceDto, prProductBasePrice>();


        //CreateMap<ShipmentHeaderDto, trShipmentHeader>().ReverseMap();
        //CreateMap<ShipmentLineDto, trShipmentLine>().ReverseMap();


        //CreateMap<UserForRegisterDto,cdUser>();
        //CreateMap<UserForRegisterDto, UserDto>();


        //#region Ürün model mapping

        //CreateMap<cdProductDesc, ProductDto>()
        //    .ReverseMap();

        //CreateMap<cdProduct, ProductDto>()
        //    .ReverseMap();

        //CreateMap<ProductDto, cdProduct>()
        //    .ForMember(x => x.BrandId, y => y.MapFrom(z => z.Brand.BrandId));

        //#endregion
    }
}
