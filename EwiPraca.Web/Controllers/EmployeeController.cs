using AutoMapper;
using Ewipraca.ImportProcessors;
using EwiPraca.App_Start.Identity;
using EwiPraca.Enumerations;
using EwiPraca.Exporters;
using EwiPraca.Importers;
using EwiPraca.Model;
using EwiPraca.Models;
using EwiPraca.Services.Interfaces;
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEwiFileService _fileService;
        private readonly IUserCompanyService _userCompanyService;
        private readonly IAddressService _addressService;
        private readonly IEwiImporter _companyEmployeeImporter;
        private readonly IEwiExporter _companyEmployeeExporter;
        private readonly IImportEmployeeProcessor _employeeProcessor;
        private readonly IPositionDictionaryService _positionDictionaryService;
        private readonly IJobPartDictionaryService _jobPartDictionaryService;
        private readonly ApplicationUserManager _userManager;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public EmployeeController(IEmployeeService employeeService,
            IUserCompanyService userCompanyService,
            IAddressService addressService,
            IEwiImporter companyEmployeeImporter,
            IEwiExporter companyEmployeeExporter,
            IImportEmployeeProcessor employeeProcessor,
            IPositionDictionaryService positionDictionaryService,
            ApplicationUserManager userManager,
            IJobPartDictionaryService jobPartDictionaryService,
            IEwiFileService fileService)
        {
            _employeeService = employeeService;
            _userCompanyService = userCompanyService;
            _addressService = addressService;
            _companyEmployeeImporter = companyEmployeeImporter;
            _companyEmployeeExporter = companyEmployeeExporter;
            _employeeProcessor = employeeProcessor;
            _positionDictionaryService = positionDictionaryService;
            _jobPartDictionaryService = jobPartDictionaryService;
            _userManager = userManager;
            _fileService = fileService;
        }

        public ActionResult Index(int id, EmployeeListTypes viewType)
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
            model.Employees = Mapper.Map<List<EmployeeViewModel>>(company.Employees.Where(x => !x.IsDeleted));

            string viewName = string.Empty;

            switch (viewType)
            {
                case EmployeeListTypes.MedicalResults:
                    viewName = "MedicalResults";
                    break;
                case EmployeeListTypes.OSHTrainings:
                    viewName = "OSHTrainings";
                    break;
                case EmployeeListTypes.SickLeaves:
                    viewName = "SickLeaves";
                    break;
                case EmployeeListTypes.Default:
                    viewName = "Index";
                    break;

            }

            return View(viewName, model);
        }


        [HttpGet]
        public ActionResult MoveEmployeeView(int employeeId)
        {
            var employee = _employeeService.GetById(employeeId);
            var companies = employee.UserCompany.ApplicationUser.UserCompanies.Where(x => x.Id != employee.UserCompanyId).ToList();

            return PartialView("_MoveEmployeeModal", new MoveEmployeeViewModel() { EmployeeId = employeeId, Companies = companies });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult MoveEmployee(MoveEmployeeViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    var employee = _employeeService.GetById(model.EmployeeId);

                    employee.UpdatedDate = DateTime.Now;
                    employee.UserCompanyId = model.TargetCompanyId;
                    _employeeService.Update(employee);
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
        public ActionResult AddEmployeeView(int companyId)
        {
            var addressType = _addressService.GetAddressTypeByName(Enumerations.AddressType.Zameldowania.ToString());

            var positions = _positionDictionaryService.GetByUserCompanyId(companyId)?.Values;

            return PartialView("_AddEmployeeModal", new EmployeeViewModel() { UserCompanyId = companyId, Positions = positions, Address = new AddressViewModel() { AddressTypeId = addressType.Id } });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEmployee(EmployeeViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    var employee = Mapper.Map<Employee>(model);

                    employee.CreatedDate = DateTime.Now;
                    employee.UpdatedDate = employee.CreatedDate;

                    _employeeService.Create(employee);

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    logger.Error(e, e.Message);
                    result = new { Success = "false", Message = WebResources.ErrorMessage };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;

                result = new { Success = "false", Message = error };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditEmployee(EmployeeViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    var employee = Mapper.Map<Employee>(model);

                    employee.UpdatedDate = DateTime.Now;

                    _employeeService.Update(employee);

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    logger.Error(e, e.Message);
                    result = new { Success = "false", Message = WebResources.ErrorMessage };
                }
            }
            else
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;

                result = new { Success = "false", Message = error };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditEmployeeView(int employeeId)
        {
            var employee = _employeeService.GetById(employeeId);

            var employeeViewModel = Mapper.Map<EmployeeViewModel>(employee);

            employeeViewModel.Positions = _positionDictionaryService.GetByUserCompanyId(employee.UserCompanyId)?.Values;

            return PartialView("_EditEmployeeModal", employeeViewModel);
        }

        [HttpGet]
        public ActionResult AddPositionDictionaryView(int companyId)
        {
            PositionDictionaryViewModel positionDictionaryViewModel;

            var dictionary = _positionDictionaryService.GetByUserCompanyId(companyId);

            if (dictionary != null)
            {
                positionDictionaryViewModel = Mapper.Map<PositionDictionaryViewModel>(dictionary);
            }
            else
            {
                positionDictionaryViewModel = new PositionDictionaryViewModel() { UserCompanyId = companyId };
            }

            return PartialView("_AddPositionDictionaryViewModal", positionDictionaryViewModel);
        }

        [HttpGet]
        public ActionResult AddPositionDictionaryValueView(int dictionaryId, int companyId)
        {
            var positionDictionaryValueViewModel = new PositionDictionaryValueViewModel() { PositionDictionaryId = dictionaryId, UserCompanyId = companyId };

            return PartialView("_AddPositionDictionaryValueViewModal", positionDictionaryValueViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPositionDictionaryValue(PositionDictionaryValueViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.PositionDictionaryId > 0)
                    {
                        var positionValue = Mapper.Map<PositionDictionaryValue>(model);

                        _positionDictionaryService.CreateDictionaryValue(positionValue);
                    }
                    else
                    {
                        PositionDictionary dictionary = new PositionDictionary()
                        {
                            UserCompanyId = model.UserCompanyId,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now
                        };

                        int dictionaryId = _positionDictionaryService.Create(dictionary);

                        var positionValue = Mapper.Map<PositionDictionaryValue>(model);
                        positionValue.PositionDictionaryId = dictionaryId;

                        _positionDictionaryService.CreateDictionaryValue(positionValue);
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e, e.Message);
                    result = new { Success = "false", Message = WebResources.ErrorMessage };
                }
            }
            else
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;

                result = new { Success = "false", Message = error };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditPositionDictionaryValueView(int id, int companyId)
        {
            var dictValue = _positionDictionaryService.GetPositionValueById(id);

            var positionDictionaryValueViewModel = Mapper.Map<PositionDictionaryValueViewModel>(dictValue);

            positionDictionaryValueViewModel.UserCompanyId = companyId;

            return PartialView("_EditPositionDictionaryValueViewModal", positionDictionaryValueViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPositionDictionaryValue(PositionDictionaryValueViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    var positionValue = _positionDictionaryService.GetPositionValueById(model.Id);

                    positionValue.Name = model.Name;
                    positionValue.Description = model.Description;

                    positionValue.PositionDictionary.UpdatedDate = DateTime.Now;

                    _positionDictionaryService.UpdateDictionaryValue(positionValue);

                }
                catch (Exception e)
                {
                    logger.Error(e, e.Message);
                    result = new { Success = "false", Message = WebResources.ErrorMessage };
                }
            }
            else
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;

                result = new { Success = "false", Message = error };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddJobPartDictionaryView(int companyId)
        {
            JobPartDictionaryViewModel jobPartDictionaryViewModel;

            var dictionary = _jobPartDictionaryService.GetByUserCompanyId(companyId);

            if (dictionary != null)
            {
                jobPartDictionaryViewModel = Mapper.Map<JobPartDictionaryViewModel>(dictionary);
            }
            else
            {
                jobPartDictionaryViewModel = new JobPartDictionaryViewModel() { UserCompanyId = companyId };
            }

            return PartialView("_AddJobPartDictionaryViewModal", jobPartDictionaryViewModel);
        }

        [HttpGet]
        public ActionResult AddJobPartDictionaryValueView(int dictionaryId, int companyId)
        {
            var jobPartDictionaryValueViewModel = new JobPartDictionaryValueViewModel() { JobPartDictionaryId = dictionaryId, UserCompanyId = companyId };

            return PartialView("_AddJobPartDictionaryValueViewModal", jobPartDictionaryValueViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddJobPartDictionaryValue(JobPartDictionaryValueViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.JobPartDictionaryId > 0)
                    {
                        var jobPartValue = Mapper.Map<JobPartDictionaryValue>(model);

                        _jobPartDictionaryService.CreateDictionaryValue(jobPartValue);
                    }
                    else
                    {
                        JobPartDictionary dictionary = new JobPartDictionary()
                        {
                            UserCompanyId = model.UserCompanyId,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now
                        };

                        int dictionaryId = _jobPartDictionaryService.Create(dictionary);

                        var positionValue = Mapper.Map<JobPartDictionaryValue>(model);
                        positionValue.JobPartDictionaryId = dictionaryId;

                        _jobPartDictionaryService.CreateDictionaryValue(positionValue);
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e, e.Message);
                    result = new { Success = "false", Message = WebResources.ErrorMessage };
                }
            }
            else
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;

                result = new { Success = "false", Message = error };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditJobPartDictionaryValueView(int id, int companyId)
        {
            var dictValue = _jobPartDictionaryService.GetJobPartValueById(id);

            var jobPartDictionaryValueViewModel = Mapper.Map<JobPartDictionaryValueViewModel>(dictValue);

            jobPartDictionaryValueViewModel.UserCompanyId = companyId;

            return PartialView("_EditJobPartDictionaryValueViewModal", jobPartDictionaryValueViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditJobPartDictionaryValue(JobPartDictionaryValueViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    var positionValue = _jobPartDictionaryService.GetJobPartValueById(model.Id);

                    positionValue.Name = model.Name;
                    positionValue.Notes = model.Notes;

                    positionValue.JobPartDictionary.UpdatedDate = DateTime.Now;

                    _jobPartDictionaryService.UpdateDictionaryValue(positionValue);

                }
                catch (Exception e)
                {
                    logger.Error(e, e.Message);
                    result = new { Success = "false", Message = WebResources.ErrorMessage };
                }
            }
            else
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;

                result = new { Success = "false", Message = error };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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


        [HttpGet]
        public ActionResult ExportToExcelConfirmationView(int companyId)
        {
            return PartialView("_ExportToExcelConfirmationModal", new ExportEmployeesConfirmationViewModel() { CompanyId = companyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportEmployeesToExcel(ExportEmployeesConfirmationViewModel model)
        {
            var result = new { Success = "true", Message = "Success", FileGuid = string.Empty, FileName = string.Empty };

            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                var userName = _userManager.Users.FirstOrDefault(x => x.Id == userId).UserName;

                var loggedinUser = _userManager.Find(userName, model.Password);

                if (loggedinUser != null)
                {
                    try
                    {
                        var employees = _employeeService.GetByCompanyId(model.CompanyId);

                        string handle = Guid.NewGuid().ToString();

                        var ms = _companyEmployeeExporter.ExportEmployees(employees, handle);

                        TempData[handle] = ms.ToArray();

                        result = new { Success = "true", Message = "Success", FileGuid = handle, FileName = "EwiPracaExportPracownikow.xlsx" };
                    }
                    catch (Exception e)
                    {
                        logger.Error(e, e.Message);
                        result = new { Success = "false", Message = WebResources.ErrorMessage, FileGuid = string.Empty, FileName = string.Empty };
                    }

                }
                else
                {
                    return Json(new { Success = "false", Message = "Hasło jest nieprawidłowe." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = "false", Message = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage }, JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult DownloadEmployeeExportFile(string fileGuid, string fileName)
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

                    var results = _companyEmployeeImporter.Import(newfullFileName, model.CompanyId);

                    if (results.Count > 0)
                    {
                        var employees = Mapper.Map<List<Employee>>(results);

                        _employeeProcessor.Process(employees, model.CompanyId, model.IsOverride);
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e, e.Message);
                    result = new { Success = "false", Message = WebResources.ErrorMessage };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = "false", Message = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage }, JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteEmployee(int id)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                var employee = _employeeService.GetById(id);

                if (employee != null)
                {
                    _employeeService.Delete(employee);
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
        public JsonResult DeletePositionDictionaryValue(int id)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                var positionValue = _positionDictionaryService.GetPositionValueById(id);

                if (positionValue != null && !_positionDictionaryService.IsPositionInUse(positionValue))
                {
                    _positionDictionaryService.DeleteDictionaryValue(positionValue);
                }
                else
                {
                    result = new { Success = "false", Message = "Nie można skasować stanowiska, które jest już w użyciu." };
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
        public JsonResult DeleteJobPartDictionaryValue(int id)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                var positionValue = _jobPartDictionaryService.GetJobPartValueById(id);

                if (positionValue != null && !_jobPartDictionaryService.IsPositionInUse(positionValue))
                {
                    _jobPartDictionaryService.DeleteDictionaryValue(positionValue);
                }
                else
                {
                    result = new { Success = "false", Message = "Nie można skasować etatu, który jest już w użyciu." };
                }
            }
            catch (Exception e)
            {
                result = new { Success = "false", Message = e.Message };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadFile(int employeeId, int fileType)
        {
            foreach (string item in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
                if (file.ContentLength == 0)
                    continue;
                if (file.ContentLength > 0)
                {
                    try
                    {
                        string fileName = file.FileName;
                        string uploadPath = "C:/MediaFiles/";

                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        string guidFolder = Guid.NewGuid().ToString();
                        string path = uploadPath + guidFolder;

                        Directory.CreateDirectory(path);

                        path = path + "/" + fileName;

                        string extension = Path.GetExtension(file.FileName);

                        file.SaveAs(path);

                        var now = DateTime.Now;

                        EwiFile newFile = new EwiFile()
                        {
                            ParentObjectId = employeeId,
                            FileName = path,
                            FileType = (FileType)fileType,
                            CreatedDate = now,
                            UpdatedDate = now,
                            ContentType = file.ContentType
                        };

                        _fileService.Create(newFile);
                    }
                    catch (Exception e)
                    {
                        logger.Error(e, e.Message);
                    }
                }
            }



            return Json("");
        }

    }
}