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
    public class SickLeaveController : Controller
    {
        private readonly ISickLeaveService _sickLeaveService;
        private readonly IEmployeeService _employeeService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public SickLeaveController(ISickLeaveService sickLeaveService,
            IEmployeeService employeeService)
        {
            _sickLeaveService = sickLeaveService;
            _employeeService = employeeService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddSickLeaveView(int employeeId)
        {
            return PartialView("_AddSickLeaveModal", new SickLeaveViewModel() { EmployeeId = employeeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddSickLeave(SickLeaveViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    //if (model.DateFrom > model.DateTo)
                    //{
                    //    result = new { Success = "false", Message = "Data początku zwolnienia nie może być późniejsza niż data końca zwolnienia." };

                    //    return Json(result, JsonRequestBehavior.AllowGet);
                    //}

                    //var employee = _employeeService.GetById(model.EmployeeId);
                    //var lastLeave = employee.SickLeaves?.Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();

                    //if ((lastLeave != null && model.DateFrom < lastLeave.DateTo && lastLeave.DateFrom < model.DateTo))
                    //{
                    //    result = new { Success = "false", Message = "Data ważności nowego szkolenia musi być późniejsza od daty ważności poprzedniego." };

                    //    return Json(result, JsonRequestBehavior.AllowGet);
                    //}

                    var sickLeave = Mapper.Map<SickLeave>(model);

                    sickLeave.CreatedDate = DateTime.Now;
                    sickLeave.UpdatedDate = sickLeave.CreatedDate;

                    _sickLeaveService.Create(sickLeave);
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
        public JsonResult DeleteSickLeave(int id)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                var sickLeave = _sickLeaveService.GetById(id);

                if (sickLeave != null)
                {
                    _sickLeaveService.Delete(sickLeave);
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