using System;
using System.Configuration;
using System.Linq;
using NLog;
using ProviderService.Providers;
using ProviderService.Updaters;
using Quartz;
using TestTask.Common.DAL.UOW;

namespace ProviderService.Jobs
{
    public class PressureUpdateJob : IJob
    {
        private const string CounterType = "Pressure";
        private readonly IUpdater _updater;
        private IProvider _provider;

        public PressureUpdateJob()
        {
            _updater = new Updater(CounterType, new UnitOfWork());
        }

        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _log.Trace("Starting new task");
                var baseAddress = ConfigurationManager.AppSettings["api:baseAddress"];
                _log.Info("baseAddress: {0}", baseAddress);

                // get last measurement
                DateTime fixationDate;
                using (var uow = new UnitOfWork())
                {
                    var lastCounter = uow.CountersRepository
                        .Where(c => c.CounterType == CounterType)
                        .OrderByDescending(c => c.FixationDate)
                        .FirstOrDefault();
                    fixationDate = lastCounter != null ? lastCounter.FixationDate : DateTime.Now.AddDays(-1);
                }

                // combine query string
                var measurementAddress = string.Format(ConfigurationManager.AppSettings["api:getPressureAddress"],
                    fixationDate.ToUniversalTime().ToString("MM-dd-yy hh:mm"));
                
                _log.Info("measurementAddress: {0}", measurementAddress);
                _provider = new WebApiProvider(baseAddress, measurementAddress);

                _updater.Update(_provider);
                _log.Trace("Task done");
            }
            catch (Exception ex)
            {
                _log.Warn("PressureUpdateJob exception {0}", ex.Message);
            }
        }
    }
}
