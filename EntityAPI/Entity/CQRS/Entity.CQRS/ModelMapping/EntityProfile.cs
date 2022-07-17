using AutoMapper;
using Entity.ResponseModels;

namespace Entity.CQRS.ModelMapping
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<Models.Entity, GetEntityResponseModel>();
        }
    }
}
