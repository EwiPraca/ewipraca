using AutoMapper;
using EwiPraca.Importers;
using EwiPraca.Model;
using EwiPraca.Models;
using EwiPraca.Services.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IUserCompanyService _userCompanyService;
        private readonly IAddressService _addressService;
        private readonly CompanyEmployeeImporter _companyEmployeeImporter;

        public EmployeeController(IEmployeeService employeeService,
            IUserCompanyService userCompanyService,
            IAddressService addressService,
            CompanyEmployeeImporter companyEmployeeImporter)
        {
            _employeeService = employeeService;
            _userCompanyService = userCompanyService;
            _addressService = addressService;
            _companyEmployeeImporter = companyEmployeeImporter;
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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

        [HttpGet]
        public ActionResult PostExcelTemplate()
        {
            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream(WebResources.EwiPracaImportPracownikowSzablon))
            {
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            var result = new { FileGuid = handle, FileName = "EwiPracaImportPracownikowSzablon.xlsx" };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult DownloadEmployeeImportExcelTemplate(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpGet]
        public ActionResult ImportEmployeesFromExcelView(int companyId)
        {
            return PartialView("_ImportEmployeesFromExcelModal", new ImportEmployeesFromExcelViewModel() { CompanyId = companyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportEmployeesFromExcel(ImportEmployeesFromExcelViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                var folder = WebResources.ImportEmployeesExcelTemplatesFolderPath;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string extension = Path.GetExtension(model.SpreadsheetFile.FileName);

                var newFileName = Guid.NewGuid().ToString();
                var newfullFileName = Path.Combine(folder, newFileName + extension);
                try
                {
                    model.SpreadsheetFile.SaveAs(newfullFileName);
                    _companyEmployeeImporter.Import(newfullFileName, model.CompanyId);
                }
                catch (Exception e)
                {
                    return Json(new { Success = "false", Message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = "false", Message = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage }, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index", "Employee", new { id = model.CompanyId });
        }
    }
}