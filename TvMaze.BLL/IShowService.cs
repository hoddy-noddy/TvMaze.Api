using System.Collections.Generic;
using System.Threading.Tasks;
using TvMaze.BLL.Dto;

namespace TvMaze.BLL
{
    public interface IShowService
    {
        Task<List<ShowDto>> GetAllShows(int pageSize, int pageNumber);
    }
}