using System.Threading;
using System.Threading.Tasks;

namespace TvMaze.BLL
{
    public interface ITvMazeService
    {
        Task UpdateDatabaseWitShowInformationAsync(CancellationToken cancellationToken);
    }
}