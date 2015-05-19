using ProviderService.Providers;

namespace ProviderService.Updaters
{
    public interface IUpdater
    {
        void Update(IProvider provider);
    }
}