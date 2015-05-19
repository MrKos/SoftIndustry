using TestTask.Common.DAL.Models;

namespace ProviderService.Models
{
    public class MeasurementCollection
    {
        private Measurement[] _measurements;
        private string[] _measurementKeys;

        public MeasurementCollection(Measurement[] measurements)
        {
            Measurements = measurements;
        }

        public Measurement[] Measurements {
            get { return _measurements; }
            private set { _measurements = value; }
        }

        public string[] MeasurementKeys
        {
            get { return _measurementKeys; }
            set { _measurementKeys = value; }
        }
    }
}
