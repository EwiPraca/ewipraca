using AutoMapper;
using EwiPraca.Model;
using EwiPraca.Models;
using EwiPraca.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    public class SickLeaveController : Controller
    {
        private readonly ISickLeaveService _sickLeaveService;

        public SickLeaveController(ISickLeaveService sickLeaveService,
            IEmployeeService employeeService)
        {
            _sickLeaveService = sickLeaveService;
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
                    var sickLeave = Mapper.Map<SickLeave>(model);

                    sickLeave.CreatedDate = DateTime.Now;
                    sickLeave.UpdatedDate = sickLeave.CreatedDate;

                    _sickLeaveService.Create(sickLeave);
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
                result = new { Success = "false", Message = e.Message };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}