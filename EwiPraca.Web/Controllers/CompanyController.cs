using AutoMapper;
using EwiPraca.App_Start.Identity;
using EwiPraca.Attributes;
using EwiPraca.Models;
using EwiPraca.Services.Interfaces;
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    [CookieConsent]
    public class CompanyController : Controller
    {
        private readonly IUserCompanyService _userCompanyService;
        private readonly IAddressService _addressService;
        private readonly ApplicationUserManager _applicationUserManager;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public CompanyController(ApplicationUserManager applicationUserManager,
            IUserCompanyService userCompanyService,
            IAddressService addressService)
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
        
        public ActionResult GetUserCompanies(string userId)
        {
            var companies = _userCompanyService.GetUserCompanies(userId);

            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            foreach(var company in companies)
            {
                dictionary.Add(company.CompanyName, company.Id);
            }

            return PartialView("UserCompaniesMenu", dictionary);
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
                        company.Notes = model.Notes;

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