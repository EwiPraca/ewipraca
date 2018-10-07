﻿using AutoMapper;
using EwiPraca.App_Start.Identity;
using EwiPraca.Model.UserArea;
using EwiPraca.Models;
using EwiPraca.Services.Services;
using Microsoft.AspNet.Identity;
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

                    company = Mapper.Map<UserCompany>(model);

                    company.UpdatedDate = DateTime.Now;
                    _userCompanyService.Update(company);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("NotFound", e.Message);
                }
            }

            return View(model);
        }
    }
}