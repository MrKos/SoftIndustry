using System.Threading;

namespace ProviderService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            var providerService = new ProviderServiceSvc();
            providerService.OnDebug();

            // waiting infinite
            Thread.Sleep(Timeout.Infinite);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new ProviderServiceSvc() 
            };
            ServiceBase.Run(ServicesToRun); 
#endif
        }
    }
}
