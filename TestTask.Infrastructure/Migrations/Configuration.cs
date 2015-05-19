using System.Collections.Generic;
using TestTask.Common.DAL.DatabaseModels;

namespace TestTask.Common.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TestTask.Common.DAL.Context.MessureContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TestTask.Common.DAL.Context.MessureContext context)
        {
            context.Counters.AddRange(getCounters());
        }

        private IEnumerable<Counter> getCounters()
        {
            return new List<Counter>
            {
                new Counter
                {
                    CounterType = "Pressure",
                    FixationDate = DateTime.Parse("18-05-2015 10:01:38"),
                    Value = 33.4
                },
                new Counter
                {
                    CounterType = "Pressure",
                    FixationDate = DateTime.Parse("18-05-2015 7:10:59"),
                    Value = 43.4
                },
                new Counter
                {
                    CounterType = "Pressure",
                    FixationDate = DateTime.Parse("18-05-2015 10:33:32"),
                    Value = 3.4
                },
                new Counter
                {
                    CounterType = "Temperature",
                    FixationDate = DateTime.Parse("18-05-2015 09:10:23"),
                    Value = 12.6
                },
                new Counter
                {
                    CounterType = "Temperature",
                    FixationDate = DateTime.Parse("18-05-2015 11:50:28"),
                    Value = 16.4
                },
                new Counter
                {
                    CounterType = "Pressure",
                    FixationDate = DateTime.Parse("19-05-2015 13:03:11"),
                    Value = 3.2
                },                new Counter
                {
                    CounterType = "Temperature",
                    FixationDate = DateTime.Parse("18-05-2015 18:10:23"),
                    Value = 28.6
                },
                new Counter
                {
                    CounterType = "Pressure",
                    FixationDate = DateTime.Parse("19-05-2015 20:50:28"),
                    Value = 36.4
                },
                new Counter
                {
                    CounterType = "Temperature",
                    FixationDate = DateTime.Parse("18-05-2015 22:03:11"),
                    Value = 17.2
                }
            };
        }
    }
}
