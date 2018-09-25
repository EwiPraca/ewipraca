using AutoMapper;
using EwiPraca.App_Start.Identity;
using EwiPraca.Models;
using EwiPraca.Services.Services;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly UserCompanyService _userCompanyService;
        private readonly ApplicationUserManager _applicationUserManager;
        public CompanyController(ApplicationUserManager applicationUserManager, 
            UserCompanyService userCompanyService)
        {
            _userCompanyService = userCompanyService;
            _applicationUserManager = applicationUserManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DisplayCompany(int id)
        {
            var company = _userCompanyService.GetById(id);

            if(company == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var userId = User.Identity.GetUserId();

            if(!userId.Equals(company.ApplicationUserID))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(Mapper.Map<UserCompanyViewModel>(company));
        }
    }
}