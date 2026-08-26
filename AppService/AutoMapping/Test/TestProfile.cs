using AutoMapper;
using Crosscutting.DTO.Test;
using Domain.Entity;

namespace AppService.AutoMapping.Test
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<CreateTestRequestDTO, TestEntity>();
            CreateMap<TestEntity, TestResponseDTO>();
        }
    }
}
