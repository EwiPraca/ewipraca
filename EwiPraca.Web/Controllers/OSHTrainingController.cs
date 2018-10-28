using AutoMapper;
using EwiPraca.Model;
using EwiPraca.Models;
using EwiPraca.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    public class OSHTrainingController : Controller
    {
        private readonly IOSHTrainingService _oshTrainingService;

        public OSHTrainingController(IOSHTrainingService oshTrainingService)
        {
            _oshTrainingService = oshTrainingService;
        }

        [HttpGet]
        public ActionResult EditOSHTrainingView(int reportId)
        {
            var mReport = _oshTrainingService.GetById(reportId);

            var mReportViewModel = Mapper.Map<OSHTrainingViewModel>(mReport);

            return PartialView("_EditOSHTrainingModal", mReportViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditOSHTraining(OSHTrainingViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                var oshTraining = Mapper.Map<OSHTraining>(model);

                oshTraining.UpdatedDate = DateTime.Now;

                _oshTrainingService.Update(oshTraining);

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
        public ActionResult AddOSHTrainingView(int employeeId)
        {
            return PartialView("_AddOSHTrainingModal", new OSHTrainingViewModel() { EmployeeId = employeeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOSHTraining(OSHTrainingViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    var oshTraining = Mapper.Map<OSHTraining>(model);

                    oshTraining.CreatedDate = DateTime.Now;
                    oshTraining.UpdatedDate = oshTraining.CreatedDate;

                    _oshTrainingService.Create(oshTraining);
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
        public JsonResult DeleteOSHTraining(int id)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                var oshTraining = _oshTrainingService.GetById(id);

                if (oshTraining != null)
                {
                    _oshTrainingService.Delete(oshTraining);
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