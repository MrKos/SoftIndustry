using System.Data.Entity;
using TestTask.Common.DAL.DatabaseModels;

namespace TestTask.Common.DAL.Context
{
    public class MessureContext : DbContext
    {
        public DbSet<Counter> Counters { get; set; }
        public DbSet<Marker> Markers { get; set; }
    }
}
