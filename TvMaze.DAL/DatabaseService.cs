using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TvMaze.DAL
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IDbContextFactory<TvMazeContext> dbContextFactory;
        public DatabaseService(IDbContextFactory<TvMazeContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<List<Show>> GetAllShows(int pageSize, int pageNumber)
        {
            using var context = dbContextFactory.CreateDbContext();

            var showList = await context.Shows.Skip(pageSize * pageNumber).Take(pageSize).ToListAsync();

            return showList;
        }

        public async Task AddOrUpdateShowAsync(Show show)
        {
            using var context = dbContextFactory.CreateDbContext();

            if(context.Shows.Any(s => s.ShowId == show.ShowId))
            {
                show.Id = context.Shows.Where(s => s.ShowId == show.ShowId).Select(s => s.Id).First();
                context.Shows.Update(show);
            }
            else
            {
                await context.Shows.AddAsync(show);
            }         
            await context.SaveChangesAsync();
        }

    }
}
