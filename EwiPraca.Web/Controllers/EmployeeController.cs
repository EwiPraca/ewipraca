using AutoMapper;
using EwiPraca.Model;
using EwiPraca.Models;
using EwiPraca.Services.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;
        private readonly UserCompanyService _userCompanyService;
        private readonly AddressService _addressService;
        public EmployeeController(EmployeeService employeeService,
            UserCompanyService userCompanyService,
            AddressService addressService)
        {
            _employeeService = employeeService;
            _userCompanyService = userCompanyService;
            _addressService = addressService;
        }
        public ActionResult Index(int id)
        {
            var company = _userCompanyService.GetById(id);

            if (company == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var userId = User.Identity.GetUserId();

            if (!userId.Equals(company.ApplicationUserID))
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new CompanyEmployeesViewModel();
            model.CompanyId = id;
            model.CompanyName = company.CompanyName;
            model.Employees = Mapper.Map<List<EmployeeViewModel>>(company.Employees);
            return View(model);
        }

        [HttpGet]
        public ActionResult AddEmployeeView(int companyId)
        {
            return PartialView("_AddEmployeeModal", new EmployeeViewModel() { UserCompanyId = companyId });
        }

        [HttpPost]
        public JsonResult EditEmployee(EmployeeViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                var employee = Mapper.Map<Employee>(model);
                
                employee.UpdatedDate = DateTime.Now;

                _employeeService.Update(employee);

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
        public ActionResult EditEmployeeView(int employeeId)
        {
            var employee = _employeeService.GetById(employeeId);

            var employeeViewModel = Mapper.Map<EmployeeViewModel>(employee);

            return PartialView("_EditEmployeeModal", employeeViewModel);
        }

        [HttpPost]
        public JsonResult AddEmployee(EmployeeViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                var employee = Mapper.Map<Employee>(model);

                employee.CreatedDate = DateTime.Now;
                employee.UpdatedDate = employee.CreatedDate;

                employee.Address.AddressType = _addressService.GetAddressTypeByName(Enumerations.AddressType.Zameldowania.ToString());

                _employeeService.Create(employee);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;

                result = new { Success = "false", Message = error };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}