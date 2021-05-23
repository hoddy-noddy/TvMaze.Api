using System.Collections.Generic;
using System.Threading.Tasks;

namespace TvMaze.DAL
{
    public interface IDatabaseService
    {
        Task<List<Show>> GetAllShows(int pageSize, int pageNumber);
        Task AddOrUpdateShowAsync(Show show);
    }
}