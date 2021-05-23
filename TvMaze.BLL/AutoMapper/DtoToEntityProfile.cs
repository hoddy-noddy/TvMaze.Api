using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TvMaze.BLL.Dto;
using TvMaze.DAL;

namespace TvMaze.BLL.AutoMapper
{
    public class DtoToEntityProfile : Profile
    {
        public DtoToEntityProfile()
        {
            CreateMap<ShowDto, Show>()
                .ForMember(dest => dest.Cast, opts => opts.MapFrom(src => JsonConvert.SerializeObject(src.Cast)));
        }
    }
}
