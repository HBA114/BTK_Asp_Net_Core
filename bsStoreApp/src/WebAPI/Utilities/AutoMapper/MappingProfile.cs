using AutoMapper;

using Entities.Dtos;
using Entities.Models;

namespace WebAPI.Utilities.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDtoForUpdate>().ReverseMap();
        CreateMap<Book, BookDto>().ReverseMap();
    }

}
