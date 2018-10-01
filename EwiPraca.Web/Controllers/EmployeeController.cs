using EwiPraca.Models;
using EwiPraca.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly UserCompanyService _userCompanyService;
        public EmployeeController(UserCompanyService userCompanyService)
        {
            _userCompanyService = userCompanyService;
        }
        public ActionResult Index(int id)
        {
            var company = _userCompanyService.GetById(id);
            var model = new CompanyEmployeesViewModel();
            model.CompanyId = id;
            model.CompanyName = company.CompanyName;

            return View(model);
        }
    }
}