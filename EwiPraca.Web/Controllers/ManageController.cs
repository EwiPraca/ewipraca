﻿using AutoMapper;
using EwiPraca.App_Start.Identity;
using EwiPraca.Data;
using EwiPraca.Encryptor;
using EwiPraca.Model.UserArea;
using EwiPraca.Models;
using EwiPraca.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly IUserCompanyService _userCompanyService;

        public ManageController(ApplicationUserManager applicationUserManager,
            IUserCompanyService userCompanyService)
        {
            _applicationUserManager = applicationUserManager;
            _userCompanyService = userCompanyService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyProfile()
        {
            string userId = User.Identity.GetUserId();
            var userProfileModel = Mapper.Map<UserViewModel>(_applicationUserManager.Users.FirstOrDefault(x => x.Id == userId));

            userProfileModel.UserCompanies = Mapper.Map<List<UserCompanyViewModel>>(_userCompanyService.GetUserCompanies(userId));

            return View(userProfileModel);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            string userId = User.Identity.GetUserId();
            var userProfileModel = Mapper.Map<UserViewModel>(_applicationUserManager.Users.FirstOrDefault(x => x.Id == userId));

            userProfileModel.FirstName = userProfileModel.FirstNameDecrypted;
            userProfileModel.Surname = userProfileModel.SurnameDecrypted;
            userProfileModel.Email = userProfileModel.EmailDecrypted;

            return PartialView("_EditUserModal", userProfileModel);
        }

        [HttpPost]
        public JsonResult EditProfile(UserViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                var user = _applicationUserManager.Users.FirstOrDefault(x => x.Id == model.Id);

                user.FirstName = EncryptionService.Encrypt(model.FirstName);
                user.Surname = EncryptionService.Encrypt(model.Surname);
                user.Email = EncryptionService.EncryptEmail(model.Email);
                user.UserName = user.Email;

                _applicationUserManager.Update(user);

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
        public ActionResult AddUserCompany()
        {
            return PartialView("_AddUserCompanyModal", new UserCompanyViewModel());
        }

        [HttpPost]
        public JsonResult AddUserCompany(UserCompanyViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                var newCompany = Mapper.Map<UserCompany>(model);
                
                newCompany.CreatedDate = DateTime.Now;
                newCompany.UpdatedDate = newCompany.CreatedDate;

                newCompany.ApplicationUserID = User.Identity.GetUserId();

                var companyAddress = Mapper.Map<UserCompanyAddress>(model.UserCompanyAddress);

                int userCompanyAdressId = _userCompanyService.CreateCompanyAddress(companyAddress);

                newCompany.UserCompanyAdressId = userCompanyAdressId;

                _userCompanyService.Create(newCompany);

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