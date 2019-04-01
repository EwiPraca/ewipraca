using AutoMapper;
using EwiPraca.Attributes;
using EwiPraca.Model;
using EwiPraca.Model.EmployeeArea;
using EwiPraca.Models;
using EwiPraca.Models.Calendar;
using EwiPraca.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    [CookieConsent]
    public class CalendarController : Controller
    {
        private readonly ILeaveService _leaveService;
        private readonly IOSHTrainingService _oshTrainingService;
        private readonly IMedicalReportService _medicalReportService;
        private readonly ICustomEventService _customEventService;
        private readonly IUserCompanyService _companyService;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private const string noDescription = "Brak opisu";

        public CalendarController(ILeaveService leaveService,
            IOSHTrainingService oshTrainingService,
            IMedicalReportService medicalReportService,
            ICustomEventService customEventService,
            IUserCompanyService companyService)
        {
            _leaveService = leaveService;
            _medicalReportService = medicalReportService;
            _oshTrainingService = oshTrainingService;
            _customEventService = customEventService;
            _companyService = companyService;
        }

        [HttpGet]
        public ActionResult Index(int companyId)
        {
            return View(new CompanyEmployeesViewModel() { CompanyId = companyId });
        }

        [HttpGet]
        public ActionResult GetCalendarLink(int companyId)
        {
            var company = _companyService.GetById(companyId);

            if(company != null)
            {
                if(company.CalendarGuid == null)
                {
                    company.CalendarGuid = Guid.NewGuid();

                    _companyService.Update(company);
                }

                return Json(new { Success = "true", Message = Url.Action("ShowCalendar", "Manage", new { guid = company.CalendarGuid }, Request.Url.Scheme) }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = "false", Message = "An error occured." });
        }

        [HttpGet]
        public ActionResult GetCustomEventView(int companyId, DateTime startDate)
        {
            return PartialView("CustomEventView", new CustomEventViewModel() { CompanyId = companyId, StartDate = startDate });
        }

        [HttpPost]
        public ActionResult AddEvent(CustomEventViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                if ( model.StartDate >= model.EndDate)
                {
                    result = new { Success = "false", Message = "Data końca musi być późniejsza niż data rozpoczęcia." };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                try
                {
                    var customEvent = Mapper.Map<CustomEvent>(model);

                    customEvent.CreatedDate = DateTime.Now;
                    customEvent.UpdatedDate = customEvent.CreatedDate;

                    _customEventService.Create(customEvent);
                }
                catch (Exception e)
                {
                    logger.Error(e, e.Message);
                    result = new { Success = "false", Message = WebResources.ErrorMessage };
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;

                result = new { Success = "false", Message = error };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
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

            HandleLeaves(items, companyId);
            HandleMedicalReports(items, companyId);
            HandleOSHTrainings(items, companyId);
            HandleCustomEvents(items, companyId);

            return items;
        }

        private void HandleCustomEvents(List<CalendarItem> items, int companyId)
        {
            var customEvents = _customEventService.GetByCompanyId(companyId);

            foreach (var customEvent in customEvents)
            {
                items.Add(new CalendarItem()
                {
                    Id = customEvent.Id,
                    Desc = customEvent.Description ?? noDescription,
                    Title = customEvent.Title,
                    StartDate = customEvent.StartDate.ToString(),
                    EndDate = customEvent.EndDate.ToString(),
                    Color = CalendarEventColors.CustomEvent
                });
            }
        }

        private void HandleOSHTrainings(List<CalendarItem> items, int companyId)
        {
            var trainings = _oshTrainingService.GetByCompanyId(companyId);

            foreach (var training in trainings.Where(x => x.NextCompletionDate != null))
            {
                items.Add(new CalendarItem()
                {
                    Id = training.Id,
                    Desc = training.Notes ?? noDescription,
                    Title = string.Format("Upływa termin szkolenia BHP - {0} {1}", training.Employee.FirstName, training.Employee.Surname),
                    StartDate = training.NextCompletionDate.ToString(),
                    EndDate = training.NextCompletionDate.ToString(),
                    Color = CalendarEventColors.OSHTraining
                });
            }
        }

        private void HandleMedicalReports(List<CalendarItem> items, int companyId)
        {
            var medicalReports = _medicalReportService.GetByCompanyId(companyId);

            foreach (var medicalReport in medicalReports.Where(x => x.NextCompletionDate != null))
            {
                items.Add(new CalendarItem()
                {
                    Id = medicalReport.Id,
                    Desc = medicalReport.Notes ?? noDescription,
                    Title = string.Format("Upływa termin badań lekarskich - {0} {1}", medicalReport.Employee.FirstName, medicalReport.Employee.Surname),
                    StartDate = medicalReport.NextCompletionDate.ToString(),
                    EndDate = medicalReport.NextCompletionDate.ToString(),
                    Color = CalendarEventColors.MedicalReport
                });
            }
        }

        private void HandleLeaves(List<CalendarItem> items, int companyId)
        {
            var leaves = _leaveService.GetByCompanyId(companyId);

            foreach (var leave in leaves)
            {
                items.Add(new CalendarItem()
                {
                    Id = leave.Id,
                    Desc = leave.Notes ?? noDescription,
                    Title = CreateLeaveTitle(leave),
                    StartDate = leave.DateFrom.ToString(),
                    EndDate = leave.DateTo.ToString(),
                    Color = leave.LeaveType == Enumerations.LeaveType.SickLeave ? CalendarEventColors.SickLeave : CalendarEventColors.Leave
                });
            }
        }

        private string CreateLeaveTitle(Leave leave)
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