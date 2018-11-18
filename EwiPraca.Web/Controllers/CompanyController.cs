using AutoMapper;
using EwiPraca.App_Start.Identity;
using EwiPraca.Models;
using EwiPraca.Services.Services;
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly UserCompanyService _userCompanyService;
        private readonly AddressService _addressService;
        private readonly ApplicationUserManager _applicationUserManager;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public CompanyController(ApplicationUserManager applicationUserManager,
            UserCompanyService userCompanyService,
            AddressService addressService)
        {
            _userCompanyService = userCompanyService;
            _addressService = addressService;
            _applicationUserManager = applicationUserManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DisplayCompany(int id)
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

            return View(Mapper.Map<UserCompanyViewModel>(company));
        }

        [HttpPost]
        public ActionResult DisplayCompany(UserCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var company = _userCompanyService.GetById(model.Id);

                    if (company == null)
                    {
                        ModelState.AddModelError("NotFound", "Nie znaleziono firmy.");
                    }
                    else
                    {
                        company.KRS = model.KRS;
                        company.NIP = model.NIP;
                        company.REGON = company.REGON;

                        company.Address.City = model.UserCompanyAddress.City;
                        company.Address.StreetName = model.UserCompanyAddress.StreetName;
                        company.Address.StreetNumber = model.UserCompanyAddress.StreetNumber;
                        company.Address.PlaceNumber = model.UserCompanyAddress.PlaceNumber;
                        company.Address.ZIPCode = model.UserCompanyAddress.ZIPCode;

                        company.UpdatedDate = DateTime.Now;
                        _userCompanyService.Update(company);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", WebResources.ErrorMessage);
                    logger.Error(e, e.Message);
                }
            }

            return View(model);
        }
    }
}