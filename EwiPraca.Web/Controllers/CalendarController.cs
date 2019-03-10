using EwiPraca.Attributes;
using EwiPraca.Enumerations;
using EwiPraca.Model;
using EwiPraca.Models;
using EwiPraca.Models.Calendar;
using EwiPraca.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    [CookieConsent]
    public class CalendarController : Controller
    {
        private readonly ILeaveService _leaveService;

        public CalendarController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpGet]
        public ActionResult Index(int companyId)
        {
            return View(new CompanyEmployeesViewModel() { CompanyId = companyId });
        }

        [HttpGet]
        public ActionResult GetCalendarData(int companyId)
        {
            JsonResult result = new JsonResult();

            try
            {
                List<CalendarItem> data = LoadData(companyId);

                result = Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return result;
        }

        private List<CalendarItem> LoadData(int companyId)
        {
            List<CalendarItem> items = new List<CalendarItem>();

            var leaves = _leaveService.GetByCompanyId(companyId);

            foreach (var leave in leaves)
            {
                items.Add(new CalendarItem()
                {
                    Id = leave.Id,
                    Desc = leave.Notes,
                    Title = CreateTitle(leave),
                    StartDate = leave.DateFrom.ToString(),
                    EndDate = leave.DateTo.ToString()
                });
            }

            return items;
        }

        private string CreateTitle(Leave leave)
        {
            var type = leave.LeaveType.GetAttribute<DisplayAttribute>().Name;

            return string.Format("{0} ({1} - {2}) - {3}", type, leave.DateFrom.ToShortDateString(), leave.DateTo.ToShortDateString(), GetName(leave.Employee));
        }

        private string GetName(Employee employee)
        {
            return string.Format("{0} {1}", employee.FirstName, employee.Surname);
        }
    }
}