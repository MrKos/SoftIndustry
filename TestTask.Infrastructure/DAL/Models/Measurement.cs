using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TestTask.Common.DAL.Models
{
    [DataContract]
    public class Measurement
    {
        [XmlElement("date")]
        public string StringDate
        {
            get { return Date.ToString("yyyy-MM-dd HH:mm"); }
            set
            {
                DateTime temp;
                if (!DateTime.TryParse(value, out temp))
                {
                    //TODO: Error handeling logic
                }
                Date = temp;
            }
        }

        [DataMember]
        [XmlIgnore]
        public DateTime Date { get; set; }
       
        [DataMember]
        [XmlElement("value")]
        public double Value { get; set; }
    }
}
