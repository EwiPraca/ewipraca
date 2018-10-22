using AutoMapper;
using EwiPraca.App_Start.Identity;
using EwiPraca.Encryptor;
using EwiPraca.Model.UserArea;
using EwiPraca.Models;
using EwiPraca.Services.Interfaces;
using EwiPraca.Services.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly AddressService _addressService;
        private readonly IUserCompanyService _userCompanyService;

        public ManageController(ApplicationUserManager applicationUserManager,
            IUserCompanyService userCompanyService,
            AddressService addressService)
        {
            _applicationUserManager = applicationUserManager;
            _userCompanyService = userCompanyService;
            _addressService = addressService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyProfile()
        {
            string userId = User.Identity.GetUserId();
            var user = _applicationUserManager.Users.FirstOrDefault(x => x.Id == userId);
            var userProfileModel = Mapper.Map<UserViewModel>(user);

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
            var addressType = _addressService.GetAddressTypeByName(Enumerations.AddressType.Zameldowania.ToString());

            return PartialView("_AddUserCompanyModal", new UserCompanyViewModel() { UserCompanyAddress = new AddressViewModel() { AddressTypeId = addressType.Id } });
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

                newCompany.Address.AddressType = _addressService.GetAddressTypeByName(Enumerations.AddressType.Zameldowania.ToString());
                newCompany.ApplicationUserID = User.Identity.GetUserId();

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