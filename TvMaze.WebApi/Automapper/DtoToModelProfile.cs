using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvMaze.BLL.Dto;
using TvMaze.WebApi.Models;

namespace TvMaze.WebApi.Automapper
{
    public class DtoToModelProfile : Profile
    {
        public DtoToModelProfile()
        {
            CreateMap<ShowDto, ShowModel>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.ShowId));
            CreateMap<PersonDto, PersonModel>();
        }
    }
}
