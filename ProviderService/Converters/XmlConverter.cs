using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestTask.Common.DAL.Models;

namespace ProviderService.Converters
{
    public class XmlConverter : IConverter
    {
        public Measurement Convert(byte[] content)
        {
            using (var stream = new MemoryStream(content))
            {
                var result = new XmlSerializer(typeof(Measurement), new XmlRootAttribute("data")).Deserialize(stream) as Measurement;
                return result;
            }
        }
    }
}
