using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class CounterDisplayModel
    {
        public string CounterType { get; set; }
        public double CounterValue { get; set; }
        [DisplayFormat(DataFormatString = "dd-MM-yy hh:mm:ss")]
        public DateTime MessureDate { get; set; }
    }
}