using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMaze.BLL.Dto;

namespace TvMaze.BLL
{
    public interface IShowService
    {
        Task<List<ShowDto>> GetAllShowsAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);
    }
}