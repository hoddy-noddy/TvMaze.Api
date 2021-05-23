using System.Threading.Tasks;

namespace TvMaze.BLL
{
    public interface ITvMazeService
    {
        Task SeedDatabaseWithShowInformation();
    }
}