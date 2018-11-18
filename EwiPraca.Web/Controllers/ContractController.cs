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
    public class ContractController : Controller
    {
        private readonly IContractService _contractService;
        private readonly IEmployeeService _employeeService;
        private readonly IJobPartDictionaryService _jobPartDictionaryService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ContractController(IContractService contractService,
            IJobPartDictionaryService jobPartDictionaryService,
            IEmployeeService employeeService)
        {
            _contractService = contractService;
            _jobPartDictionaryService = jobPartDictionaryService;
            _employeeService = employeeService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditContractView(int contractId)
        {
            var contract = _contractService.GetById(contractId);

            var contractViewModel = Mapper.Map<ContractViewModel>(contract);

            contractViewModel.JobParts = _jobPartDictionaryService.GetByUserCompanyId(contract.Employee.UserCompanyId)?.Values;

            return PartialView("_EditContractModal", contractViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditContract(ContractViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.DateTo.HasValue && model.DateFrom >= model.DateTo.Value)
                    {
                        result = new { Success = "false", Message = "Data zakończenia umowy musi być późniejsza niż data rozpoczęcia." };

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    var employee = _employeeService.GetById(model.EmployeeId);
                    var lastContract = employee.Contracts?.Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();

                    if ((lastContract != null && !lastContract.DateTo.HasValue) || (lastContract != null && lastContract.DateTo.Value > model.DateFrom))
                    {
                        result = new { Success = "false", Message = "Data rozpoczęcia nowej umowy musi być późniejsza niż data zakończenia poprzedniej." };

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    var contract = Mapper.Map<Contract>(model);

                    contract.UpdatedDate = DateTime.Now;

                    _contractService.Update(contract);

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch(Exception e)
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

        [HttpGet]
        public ActionResult AddContractView(int employeeId)
        {
            var employee = _employeeService.GetById(employeeId);
            var jobParts = _jobPartDictionaryService.GetByUserCompanyId(employee.UserCompanyId)?.Values;

            return PartialView("_AddContractModal", new ContractViewModel() { EmployeeId = employeeId, JobParts = jobParts });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddContract(ContractViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                if(model.DateTo.HasValue && model.DateFrom >=  model.DateTo.Value)
                {
                    result = new { Success = "false", Message = "Data zakończenia umowy musi być późniejsza niż data rozpoczęcia." };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var employee = _employeeService.GetById(model.EmployeeId);
                var lastContract = employee.Contracts?.Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();

                if ((lastContract != null && !lastContract.DateTo.HasValue) || (lastContract != null && lastContract.DateTo.Value > model.DateFrom))
                {
                    result = new { Success = "false", Message = "Data rozpoczęcia nowej umowy musi być późniejsza niż data zakończenia poprzedniej." };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                try
                {
                    var contract = Mapper.Map<Contract>(model);

                    contract.CreatedDate = DateTime.Now;
                    contract.UpdatedDate = contract.CreatedDate;

                    _contractService.Create(contract);
                }
                catch(Exception e)
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
        public JsonResult DeleteContract(int id)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                var contract = _contractService.GetById(id);

                if (contract != null)
                {
                    _contractService.Delete(contract);
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