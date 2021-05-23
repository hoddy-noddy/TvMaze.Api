using Microsoft.EntityFrameworkCore;
using System;

namespace TvMaze.DAL
{
    public class TvMazeContext : DbContext
    {
        public virtual DbSet<Show> Shows {get;set;}

        public TvMazeContext(DbContextOptions options) : base(options)
        {

        }
    }
}
