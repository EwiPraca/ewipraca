﻿using AutoMapper;
using EwiPraca.Model;
using EwiPraca.Models;
using EwiPraca.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    public class ContractController : Controller
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
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

            return PartialView("_EditContractModal", contractViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditContract(ContractViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                var contract = Mapper.Map<Contract>(model);

                contract.UpdatedDate = DateTime.Now;

                _contractService.Update(contract);

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
        public ActionResult AddContractView(int employeeId)
        {
            return PartialView("_AddContractModal", new ContractViewModel() { EmployeeId = employeeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddContract(ContractViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    var contract = Mapper.Map<Contract>(model);

                    contract.CreatedDate = DateTime.Now;
                    contract.UpdatedDate = contract.CreatedDate;

                    _contractService.Create(contract);
                }
                catch(Exception e)
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
                result = new { Success = "false", Message = e.Message };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}