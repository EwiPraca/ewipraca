using AutoMapper;
using EwiPraca.Model;
using EwiPraca.Models;
using EwiPraca.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    public class MedicalReportController : Controller
    {
        private readonly IMedicalReportService _medicalReportService;

        public MedicalReportController(IMedicalReportService medicalReportService)
        {
            _medicalReportService = medicalReportService;
        }

        [HttpGet]
        public ActionResult EditMedicalReportViewModelView(int reportId)
        {
            var mReport = _medicalReportService.GetById(reportId);

            var mReportViewModel = Mapper.Map<MedicalReportViewModel>(mReport);

            return PartialView("_EditMedicalReportModal", mReport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditMedicalReport(MedicalReportViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                var mReport = Mapper.Map<MedicalReport>(model);

                mReport.UpdatedDate = DateTime.Now;

                _medicalReportService.Update(mReport);

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
            return PartialView("_AddMedicalReportModal", new ContractViewModel() { EmployeeId = employeeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddContract(MedicalReportViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    var mReport = Mapper.Map<MedicalReport>(model);

                    mReport.CreatedDate = DateTime.Now;
                    mReport.UpdatedDate = mReport.CreatedDate;

                    _medicalReportService.Create(mReport);
                }
                catch (Exception e)
                {
                    result = new { Success = "false", Message = e.Message };
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
                result = new { Success = "false", Message = e.Message };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}