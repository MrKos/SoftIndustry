using System;

namespace TestTask.Common.DAL.DatabaseModels
{
    public class Counter
    {
        public int Id { get; set; }
        public DateTime FixationDate { get; set; }
        public double Value { get; set; }
        public string CounterType { get; set; }
    }
}
