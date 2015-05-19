using System;
using System.Configuration;
using System.IO;
using NLog;
using ProviderService.Jobs;
using Quartz;
using Quartz.Impl;

namespace ProviderService
{
    public class Scheduler
    {
        private static readonly Scheduler Instance = new Scheduler();
        private readonly Logger _log = LogManager.GetLogger("Sheduler");
        private static IScheduler _scheduler;

        private Scheduler()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler();
        }

        public static Scheduler GetScheduler
        {
            get { return Instance; }
        }

        public void Start()
        {
            _scheduler.Start();

            _log.Info("Scheduler started");
        }

        public void Stop()
        {
            // the scheduler will not allow this method to return 
            // until all currently executing jobs have completed. 
            _scheduler.Shutdown(true);

            _log.Info("Scheduler stopped");
        }

        public void ReloadTasks()
        {
            try
            {
                _log.Trace("Refresh the task list");
                ClearSchedulerTasks();

                // create new job for pressure upd
                var pressureJobDetail = JobBuilder.Create<PressureUpdateJob>()
                    .WithIdentity("PressureUpdator", "Updators")
                    .Build();

                // create trigger for pressure upd job
                var pressureCron = ConfigurationManager.AppSettings["trigger:PressureUpdateJob"];
                var pressureTrigger = GetTrigger(pressureCron, "pressureUpdators");

                // add task to sheduler
                _scheduler.ScheduleJob(pressureJobDetail, pressureTrigger);

                // create new job for temperature upd
                var temperatureJobDetail = JobBuilder.Create<TemperatureUpdateJob>()
                    .WithIdentity("TemperatureUpdator", "temperatureUpdators")
                    .Build();

                // create trigger for temperature upd job
                var temperatureCron = ConfigurationManager.AppSettings["trigger:TemperatureUpdateJob"];
                var temperatureTrigger = GetTrigger(temperatureCron, "Updators");

                // add task to sheduler
                _scheduler.ScheduleJob(temperatureJobDetail, temperatureTrigger);

                _log.Info("RefreshTasksList - done!");
            }
            catch (Exception ex)
            {
                _log.Error("Error while refreshing task list: {0}", ex.Message);
            }
        }

        /// <summary>clear all existing task</summary>
        private void ClearSchedulerTasks()
        {
            _log.Trace("Clear existing tasks");
            _scheduler.Clear();
        }

        private ITrigger GetTrigger(string cronExpression, string triggerName)
        {
            if (!CronExpression.IsValidExpression(cronExpression))
                throw new InvalidDataException(
                    string.Format("{0} invalid cron expression ({1})", triggerName, cronExpression)
                    );

            _log.Trace("Cron expression for {0}: {1}", triggerName, cronExpression);
            return TriggerBuilder.Create()
                .WithIdentity(triggerName)
                .WithCronSchedule(cronExpression, x => x
                    // if has missed task start
                    .WithMisfireHandlingInstructionFireAndProceed())
                .Build();
        }
    }
}
