using AutoMapper;
using WebAPIDBLayer.Models;
using WebAPITeacherAndClassroom.ApiModels;

namespace WebAPITeacherAndClassroom.Mapper
{

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Teacher, TeacherModel>().ReverseMap();
            CreateMap<Classroom, ClassroomModel>().ReverseMap();
        }
    }
}
