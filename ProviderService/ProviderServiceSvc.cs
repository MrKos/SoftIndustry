using System;
using System.ServiceProcess;
using NLog;

namespace ProviderService
{
    public partial class ProviderServiceSvc : ServiceBase
    {
        private readonly Logger _log = LogManager.GetLogger("Provider.Service");
        private readonly Scheduler _scheduler = Scheduler.GetScheduler;
        public ProviderServiceSvc()
        {
            InitializeComponent();
        }

        // for debuggin in VisualStudio
        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _log.Trace("Starting scheduler service");
                _scheduler.Start();
                _scheduler.ReloadTasks();
                _log.Trace("The service has successfully started");
            }
            catch (Exception ex)
            {
                _log.Error("ProviderService start failed: {0}", ex.Message);
            }
        }

        protected override void OnStop()
        {
            _log.Trace("Stopping scheduler service");
            _scheduler.Stop();
            _log.Info("Service successfully stopped");
        }
    }
}
