using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ProviderService.Converters;
using ProviderService.Providers;
using ProviderService.Updaters;
using Quartz;
using TestTask.Common.DAL.UOW;

namespace ProviderService.Jobs
{
    public class TemperatureUpdateJob : IJob
    {
        private const string CounterType = "Temperature";
        private readonly IUpdater _updater;
        private IProvider _provider;
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public TemperatureUpdateJob()
        {
            _updater = new Updater(CounterType, new UnitOfWork());
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _log.Trace("Starting new task");
                var options = GetFtpOptions();

                _log.Info("Ftp options: {0}", options);

                // get last measurement
                string fixationDate;
                using (var uow = new UnitOfWork())
                {
                    // TODO max id?
                    var lastIndex = uow.MarkersRepository
                        .Where(c => c.CounterType == CounterType)
                        .OrderByDescending(c=> c.Key)
                        .FirstOrDefault();
                    fixationDate = lastIndex != null ? lastIndex.Key : "";
                }

                _provider = new FTPProvider(new XmlConverter(), options)
                {
                    MaxProcessedId = fixationDate
                };
                _updater.Update(_provider);
                _log.Trace("Task done");
            }
            catch (Exception ex)
            {
                _log.Warn("TemperatureUpdateJob exception {0}", ex.Message);
            }
        }

        private FtpOptions GetFtpOptions()
        {
            return new FtpOptions
            {
                BaseAddress = ConfigurationManager.AppSettings["ftp:baseAddress"],
                Login = ConfigurationManager.AppSettings["ftp:login"],
                Password = ConfigurationManager.AppSettings["ftp:password"],
                WorkingFolder = ConfigurationManager.AppSettings["ftp:folder"],
                TempFolder = ConfigurationManager.AppSettings["ftp:tempFolder"]
            };
        }
    }
}
