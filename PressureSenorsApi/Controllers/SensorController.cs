using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http;
using TestTask.Common.DAL.Models;

namespace PressureSensorsApi.Controllers
{
    public class SensorController : ApiController
    {
        private readonly Random rnd;

        public SensorController()
        {
            rnd = new Random(2);
        }

        public IEnumerable<Measurement> Get()
        {
            return GetMeasurements();
        }

        // GET api/<controller>/
        public IEnumerable<Measurement> Get(string time)
        {
            DateTime dateTime;
            if (!DateTime.TryParseExact(time, "MM-dd-yy HH:mm",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.AssumeUniversal, out dateTime))
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            return GetMeasurements().Where(m => m.Date >= dateTime.ToUniversalTime());
        }

        #region Helpers
        private Measurement[] GetMeasurements()
        {
            var startDate = DateTime.UtcNow.AddHours(-2);
            var x = Enumerable.Range(0, (int)(DateTime.UtcNow - startDate).TotalMinutes)
                             .Select(d => new Measurement
                             {
                                 Date = startDate.AddMinutes(d),
                                 Value = Math.Round(rnd.NextDouble() * 100, 2)
                             })
                             .ToArray();
            return x;
        }
        #endregion
    }
}