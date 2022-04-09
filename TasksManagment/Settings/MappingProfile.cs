using AutoMapper;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Tasks;
using Entities.Models;

namespace TasksManagmentApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tasks, AddTasksDto>();

            CreateMap<Tasks, EditTasksDto>();
            CreateMap<EditTasksDto, Tasks>();

          

            CreateMap<AddTasksDto, Tasks>()
           .ForMember(d => d.ApplicationUserId, opt => opt.MapFrom(r => r.UserId));


            CreateMap<Tasks, TasksDetailsDto>().ReverseMap();
            CreateMap<Tasks, TasksDto>().ReverseMap();
        }
    }
}
