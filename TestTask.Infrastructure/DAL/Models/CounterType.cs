using System.Collections.Generic;

namespace TestTask.Common.DAL.Models
{
    public class CounterType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Counter> Counters { get; set; }
    }
}
