using System;

namespace TestTask.Common.DAL.Models
{
    public class Counter
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public double Value { get; set; }

        public int CountreTypeId { get; set; }
        public virtual CounterType CounterType { get; set; }
    }
}
