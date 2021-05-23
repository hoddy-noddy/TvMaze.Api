using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public async Task<List<Show>> GetAllShowsAsync(int pageSize, int pageNumber, CancellationToken cancellationToken)
        {
            using var context = dbContextFactory.CreateDbContext();

            var showList = await context.Shows.Skip(pageSize * pageNumber).Take(pageSize).ToListAsync(cancellationToken);

            return showList;
        }

        public async Task AddOrUpdateShowAsync(Show show, CancellationToken cancellationToken)
        {
            using var context = dbContextFactory.CreateDbContext();

            if(context.Shows.Any(s => s.ShowId == show.ShowId))
            {
                show.Id = context.Shows.Where(s => s.ShowId == show.ShowId).Select(s => s.Id).First();
                context.Shows.Update(show);
            }
            else
            {
                await context.Shows.AddAsync(show,cancellationToken);
            }         
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> GetLastShowIdInDatabase()
        {
            using var context = dbContextFactory.CreateDbContext();

            return await context.Shows.OrderByDescending(s => s.ShowId).Select(s=> s.ShowId).FirstOrDefaultAsync();
        }
    }
}
