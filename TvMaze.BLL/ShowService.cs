using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvMaze.BLL.Dto;
using TvMaze.DAL;

namespace TvMaze.BLL
{
    public class ShowService : IShowService
    {
        private readonly IDatabaseService databaseService;
        private readonly IMapper mapper;

        public ShowService(IDatabaseService databaseService, IMapper mapper)
        {
            this.databaseService = databaseService;
            this.mapper = mapper;
        }
        public async Task<List<ShowDto>> GetAllShows(int pageSize, int pageNumber)
        {
            var shows = await databaseService.GetAllShows(pageSize, pageNumber);
            var showsList = mapper.Map<List<ShowDto>>(shows);

            foreach(var show in showsList)
            {
                show.Cast = show.Cast.OrderBy(p => p.Birthday).ToList();
            }

            return showsList;
        }
    }
}
