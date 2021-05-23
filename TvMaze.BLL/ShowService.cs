using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public async Task<List<ShowDto>> GetAllShowsAsync(int pageSize, int pageNumber, CancellationToken cancellationToken)
        {
            var shows = await databaseService.GetAllShowsAsync(pageSize, pageNumber, cancellationToken);
            var showsList = mapper.Map<List<ShowDto>>(shows);

            foreach(var show in showsList)
            {
                show.Cast = show.Cast.OrderBy(p => p.Birthday).ToList();
            }

            return showsList;
        }
    }
}
