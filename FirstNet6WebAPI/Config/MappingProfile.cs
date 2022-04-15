using AutoMapper;
using Models;

namespace FirstNet6WebAPI
{
    /// <summary>
    /// AutoMapper配置类
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
