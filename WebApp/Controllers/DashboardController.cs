using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using TestTask.Common.DAL.DatabaseModels;
using TestTask.Common.DAL.UOW;
using WebApp.Models;
using WebGrease.Css.Extensions;

namespace WebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _uow;

        public DashboardController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        // GET: Dashboard
        public ActionResult Index(string counterType = "", DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            // save filters
            ViewBag.dateFrom = dateFrom.HasValue ? dateFrom.Value.ToString("MM-dd-yyyy") : "";
            ViewBag.dateTo = dateTo.HasValue ? dateTo.Value.ToString("MM-dd-yyyy") : "";

            // for same date
            if (dateTo.HasValue)
                dateTo = dateTo.Value.AddHours(24);

            var counters = _uow.CountersRepository.Where(c =>
                (string.IsNullOrEmpty(counterType) || c.CounterType == counterType)
                && (dateFrom == null || c.FixationDate >= dateFrom)
                && (dateTo == null || c.FixationDate <= dateTo.Value))
                .OrderBy(c => c.FixationDate);

            var model = counters.Select(c => new CounterDisplayModel
            {
                CounterType = c.CounterType,
                CounterValue = c.Value,
                MessureDate = c.FixationDate
            }).ToList();

            var counterTypes = _uow.CountersRepository.Where().Select(c => c.CounterType);
            ViewBag.counterType = new SelectList(counterTypes.Distinct());

            return View(model);
        }
    }
}