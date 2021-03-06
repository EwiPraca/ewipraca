﻿using AutoMapper;
using EwiPraca.Attributes;
using EwiPraca.Enumerations;
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
        public ActionResult Index(int companyId, bool isReadOnly = false)
        {
            var company = _companyService.GetById(companyId);

            return View(new CalendarViewModel() { CompanyId = companyId, IsReadOnly = isReadOnly, CompanyName = company.CompanyName });
        }

        [HttpGet]
        public ActionResult GetCalendarLink(int companyId)
        {
            var company = _companyService.GetById(companyId);

            if (company != null)
            {
                if (company.CalendarGuid == null)
                {
                    company.CalendarGuid = Guid.NewGuid();

                    _companyService.Update(company);
                }

                return Json(new { Success = "true", Message = Url.Action("ShowCalendar", "Calendar", new { guid = company.CalendarGuid }, Request.Url.Scheme) }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = "false", Message = "An error occured." });
        }

        [HttpGet]
        public ActionResult ShowCalendar(Guid guid)
        {
            var company = _companyService.GetByGuid(guid);

            if (company != null)
            {
                return RedirectToAction("Index", "Calendar", new { companyId = company.Id, isReadOnly = true });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult AddCustomEventView(int companyId, DateTime startDate)
        {
            return PartialView("AddCustomEventView", new CustomEventViewModel() { CompanyId = companyId, StartDate = startDate, EndDate = startDate });
        }

        [HttpGet]
        public ActionResult EditCustomEventView(int customEventId, string color)
        {
            switch (color)
            {
                case CalendarEventColors.CustomEvent:
                    var customEvent = _customEventService.GetById(customEventId);

                    var customEventModel = Mapper.Map<CustomEventViewModel>(customEvent);

                    return PartialView("EditCustomEventView", customEventModel);

                case CalendarEventColors.MedicalReport:
                    var medicalEx = _medicalReportService.GetById(customEventId);

                    CalendarEventViewModel medicalExModel = new CalendarEventViewModel
                    {
                        Description = medicalEx.Notes,
                        Title = "Koniec badań lekarskich",
                        StartDate = medicalEx.CompletionDate,
                        EndDate = medicalEx.NextCompletionDate,
                        EmployeeName = medicalEx.Employee.FullName
                    };


                    return PartialView("CalendarEventView", medicalExModel);
                case CalendarEventColors.Leave:
                case CalendarEventColors.SickLeave:
                    var leave = _leaveService.GetById(customEventId);

                    CalendarEventViewModel leaveModel = new CalendarEventViewModel
                    {
                        Description = leave.Notes,
                        Title = CreateLeaveTitle(leave),
                        StartDate = leave.DateFrom,
                        EndDate = leave.DateTo,
                        EmployeeName = leave.Employee.FullName
                    };

                    return PartialView("CalendarEventView", leaveModel);
                case CalendarEventColors.OSHTraining:
                    var oshTraining = _oshTrainingService.GetById(customEventId);

                    CalendarEventViewModel model = new CalendarEventViewModel
                    {
                        Description = oshTraining.Notes,
                        Title = "Koniec szkolenia BHP",
                        StartDate = oshTraining.CreatedDate,
                        EndDate = oshTraining.NextCompletionDate,
                        EmployeeName = oshTraining.Employee.FullName
                    };

                    return PartialView("CalendarEventView", model);
            };

            throw new Exception("Brak widoku do wyświetlenia!");
        }

        [HttpPost]
        public JsonResult DeleteCustomEvent(int id)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                var customEvent = _customEventService.GetById(id);

                if (customEvent != null)
                {
                    _customEventService.Delete(customEvent);
                }
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                result = new { Success = "false", Message = WebResources.ErrorMessage };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddEvent(CustomEventViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                if (model.StartDate > model.EndDate)
                {
                    result = new { Success = "false", Message = "Data końca musi być późniejsza niż data rozpoczęcia." };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (model.OccurencyIntervalNumber > 0 && model.OccurenceType == null)
                {
                    result = new { Success = "false", Message = "Wymagane podanie typu cykliczności." };

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

        [HttpPost]
        public ActionResult EditEvent(CustomEventViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                if (model.StartDate > model.EndDate)
                {
                    result = new { Success = "false", Message = "Data końca musi być późniejsza niż data rozpoczęcia." };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (model.OccurencyIntervalNumber > 0 && model.OccurenceType == null)
                {
                    result = new { Success = "false", Message = "Wymagane podanie typu cykliczności." };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                try
                {
                    var customEvent = Mapper.Map<CustomEvent>(model);

                    customEvent.UpdatedDate = customEvent.CreatedDate;

                    _customEventService.Update(customEvent);
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
                if (customEvent.IsOccurency)
                {
                    int multiplier = 0;

                    for (int i = 0; i < customEvent.OccurencyIntervalNumber; i++)
                    {
                        multiplier = i * (int)customEvent.OccurenceType;

                        items.Add(new CalendarItem()
                        {
                            Id = customEvent.Id,
                            Desc = customEvent.Description ?? noDescription,
                            Title = customEvent.Title,
                            StartDate = customEvent.StartDate.AddDays(multiplier).ToString("o"),
                            EndDate = customEvent.EndDate.AddDays(multiplier).ToString("o"),
                            Color = CalendarEventColors.CustomEvent,
                            Type = CalendarEventType.CustomEvent
                        });
                    }
                }
                else
                {
                    items.Add(new CalendarItem()
                    {
                        Id = customEvent.Id,
                        Desc = customEvent.Description ?? noDescription,
                        Title = customEvent.Title,
                        StartDate = customEvent.StartDate.ToString("o"),
                        EndDate = customEvent.EndDate.ToString("o"),
                        Color = CalendarEventColors.CustomEvent
                    });
                }
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
                    StartDate = training.NextCompletionDate.Value.ToString("o"),
                    EndDate = training.NextCompletionDate.Value.ToString("o"),
                    Color = CalendarEventColors.OSHTraining,
                    Type = CalendarEventType.OSHTraining
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
                    StartDate = medicalReport.NextCompletionDate.Value.ToString("o"),
                    EndDate = medicalReport.NextCompletionDate.Value.ToString("o"),
                    Color = CalendarEventColors.MedicalReport,
                    Type = CalendarEventType.MedicalReport
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
                    StartDate = leave.DateFrom.ToString("o"),
                    EndDate = leave.DateTo.ToString("o"),
                    Color = leave.LeaveType == Enumerations.LeaveType.SickLeave ? CalendarEventColors.SickLeave : CalendarEventColors.Leave,
                    Type = CalendarEventType.Leave
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