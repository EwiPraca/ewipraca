using AutoMapper;
using EwiPraca.Attributes;
using EwiPraca.Enumerations;
using EwiPraca.Model;
using EwiPraca.Models;
using EwiPraca.Models.Employee;
using EwiPraca.Services.Interfaces;
using NLog;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    [CookieConsent]
    public class LeaveController : Controller
    {
        private readonly ILeaveService _sickLeaveService;
        private readonly IEmployeeService _employeeService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public LeaveController(ILeaveService sickLeaveService,
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
        public ActionResult AddLeaveView(int employeeId, int leaveType)
        {
            return PartialView("_AddLeaveModal", new LeaveViewModel() { EmployeeId = employeeId, LeaveType = (LeaveType)leaveType });
        }

        [HttpGet]
        public ActionResult AddEmployeeLeaveView(int companyId, int leaveType)
        {
            var model = new LeaveViewModel()
            {
                LeaveType = (LeaveType)leaveType,
                Employees = _employeeService.GetByCompanyId(companyId).Select(x => new EwiPracaSelectListItem() { Id = x.Id, Name = string.Format("{0} {1}", x.FirstName, x.Surname) }).ToList()
            };

            return PartialView("_AddEmployeeLeaveModal", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddLeave(LeaveViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    var leave = Mapper.Map<Leave>(model);

                    leave.CreatedDate = DateTime.Now;
                    leave.UpdatedDate = leave.CreatedDate;

                    _sickLeaveService.Create(leave);
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