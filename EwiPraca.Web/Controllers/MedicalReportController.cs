using AutoMapper;
using EwiPraca.Model;
using EwiPraca.Models;
using EwiPraca.Services.Interfaces;
using NLog;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    public class MedicalReportController : Controller
    {
        private readonly IMedicalReportService _medicalReportService;
        private readonly IEmployeeService _employeeService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public MedicalReportController(IMedicalReportService medicalReportService,
            IEmployeeService employeeService)
        {
            _medicalReportService = medicalReportService;
            _employeeService = employeeService;
        }

        [HttpGet]
        public ActionResult EditMedicalReportView(int reportId)
        {
            var mReport = _medicalReportService.GetById(reportId);

            var mReportViewModel = Mapper.Map<MedicalReportViewModel>(mReport);

            return PartialView("_EditMedicalReportModal", mReportViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditMedicalReport(MedicalReportViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                if (model.CompletionDate >= model.NextCompletionDate)
                {
                    result = new { Success = "false", Message = "Data ważności badania nie może być wcześniejsza niż data wykonania badania." };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var employee = _employeeService.GetById(model.EmployeeId);
                var lastCheck = employee.MedicalReports?.Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();

                if ((lastCheck != null && lastCheck.NextCompletionDate > model.NextCompletionDate))
                {
                    result = new { Success = "false", Message = "Data ważności nowego badania musi być późniejsza od daty ważności poprzedniego." };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                try
                {
                    var mReport = Mapper.Map<MedicalReport>(model);

                    mReport.UpdatedDate = DateTime.Now;

                    _medicalReportService.Update(mReport);
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
        public ActionResult AddMedicalReportView(int employeeId)
        {
            return PartialView("_AddMedicalReportModal", new MedicalReportViewModel() { EmployeeId = employeeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddMedicalReport(MedicalReportViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.CompletionDate >= model.NextCompletionDate)
                    {
                        result = new { Success = "false", Message = "Data ważności badania nie może być wcześniejsza niż data wykonania badania." };

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    var employee = _employeeService.GetById(model.EmployeeId);
                    var lastCheck = employee.MedicalReports?.Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();

                    if ((lastCheck != null && lastCheck.NextCompletionDate > model.NextCompletionDate))
                    {
                        result = new { Success = "false", Message = "Data ważności nowego badania musi być późniejsza od poprzedniego." };

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    var mReport = Mapper.Map<MedicalReport>(model);

                    mReport.CreatedDate = DateTime.Now;
                    mReport.UpdatedDate = mReport.CreatedDate;

                    _medicalReportService.Create(mReport);
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
        public JsonResult DeleteMedicalReport(int id)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                var mReport = _medicalReportService.GetById(id);

                if (mReport != null)
                {
                    _medicalReportService.Delete(mReport);
                }
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                result = new { Success = "false", Message = WebResources.ErrorMessage };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}