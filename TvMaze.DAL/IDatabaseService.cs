using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TvMaze.DAL
{
    public interface IDatabaseService
    {
        Task<List<Show>> GetAllShowsAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);
        Task AddOrUpdateShowAsync(Show show, CancellationToken cancellationToken);
    }
}