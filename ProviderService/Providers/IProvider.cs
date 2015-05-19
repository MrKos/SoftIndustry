using ProviderService.Models;

namespace ProviderService.Providers
{
    public interface IProvider
    {
        MeasurementCollection GetData();
    }
}
