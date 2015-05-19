using System;
using ProviderService.Clients;
using ProviderService.Models;
using TestTask.Common.DAL.Models;

namespace ProviderService.Providers
{
    public class WebApiProvider : IProvider
    {
        private readonly string _baseAddress;
        private readonly string _measurementAddress;

        public WebApiProvider(string baseAddress, string measurementAddress)
        {
            _baseAddress = baseAddress;
            _measurementAddress = measurementAddress;
        }

        public MeasurementCollection GetData()
        {
            var measurements = new WebApiClient<Measurement[]>(new Uri(_baseAddress)).GetData(string.Format(_measurementAddress));
            return new MeasurementCollection(measurements);
        }
    }
}
