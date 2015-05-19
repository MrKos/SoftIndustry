using System.Collections.Generic;
using System.Linq;
using ProviderService.Providers;
using TestTask.Common.DAL.Models;
using TestTask.Common.DAL.UOW;
using TestTask.Common.DAL.DatabaseModels;

namespace ProviderService.Updaters
{
    public class Updater : IUpdater
    {
        private string _measurmentType;
        private IUnitOfWork _uow;

        public Updater(string measurmentType, IUnitOfWork uow)
        {
            _measurmentType = measurmentType;
            _uow = uow;
        }

        private void SaveMesurments(IEnumerable<Measurement> measurements)
        {
            foreach (var measurement in measurements)
            {
                _uow.CountersRepository.Insert(new Counter { FixationDate = measurement.Date.ToLocalTime(), Value = measurement.Value, CounterType = _measurmentType });
            }
            _uow.Save();
        }

        private void SaveMesurmentKeys(IEnumerable<string> measurementKeys)
        {
            foreach (var key in measurementKeys)
            {
                _uow.MarkersRepository.Insert(new Marker { CounterType = _measurmentType, Key = key});
            }
            _uow.Save();
        }

        public void Update(IProvider provider)
        {
            var data = provider.GetData();

            // if no data received
            if (data == null)
                return;

            // if we have measuremets counters
            if (data.Measurements != null && data.Measurements.Any())
                SaveMesurments(data.Measurements);

            // if we have measuremets keys
            if (data.MeasurementKeys != null && data.MeasurementKeys.Any())
                SaveMesurmentKeys(data.MeasurementKeys);
        }
    }
}
