using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TvMaze.BLL.Dto;
using TvMaze.DAL;

namespace TvMaze.BLL.AutoMapper
{
    public class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            CreateMap<Show, ShowDto>()
                .ForMember(dest => dest.Cast, opts => opts.MapFrom(src => JsonConvert.DeserializeObject<List<PersonDto>>(src.Cast)));
        }
       
    }
}
