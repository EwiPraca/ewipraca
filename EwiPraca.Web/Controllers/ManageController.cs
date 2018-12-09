using AutoMapper;
using EwiPraca.App_Start.Identity;
using EwiPraca.Encryptor;
using EwiPraca.Model;
using EwiPraca.Model.UserArea;
using EwiPraca.Models;
using EwiPraca.Services.Interfaces;
using EwiPraca.Services.Services;
using Microsoft.AspNet.Identity;
using NLog;
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
        private readonly ISettingService _settingService;
        private readonly AddressService _addressService;
        private readonly IUserCompanyService _userCompanyService;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly int _defaultNumberOfDays = SettingsHandler.DaysBeforeIntervalReminder;

        public ManageController(ApplicationUserManager applicationUserManager,
            IUserCompanyService userCompanyService,
            ISettingService settingService,
            AddressService addressService)
        {
            _applicationUserManager = applicationUserManager;
            _userCompanyService = userCompanyService;
            _settingService = settingService;
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
                try
                {
                    var user = _applicationUserManager.Users.FirstOrDefault(x => x.Id == model.Id);

                    user.FirstName = EncryptionService.Encrypt(model.FirstName);
                    user.Surname = EncryptionService.Encrypt(model.Surname);
                    user.Email = EncryptionService.EncryptEmail(model.Email);
                    user.UserName = user.Email;

                    _applicationUserManager.Update(user);
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
        public ActionResult EditUserSettingView(int userSettingId, int settingId, string settingValue)
        {
            UserSettingViewModel model = null;

            if (userSettingId > 0)
            {
                var setting = _settingService.GetUserSettingById(userSettingId);

                if (setting == null || User.Identity.GetUserId() != setting.ApplicationUserID)
                {
                    throw new Exception("Ustawienie nie istnieje lub próbujesz edytować nie swoje ustawienia!");
                }
                model = Mapper.Map<UserSettingViewModel>(setting);
            }
            else
            {
                var setting = _settingService.GetSettingById(settingId);

                if (setting == null)
                {
                    throw new Exception("Ustawienie nie istnieje!");
                }

                model = new UserSettingViewModel()
                {
                    Id = 0,
                    SettingId = setting.Id,
                    SettingDescription = setting.SettingDescription,
                    SettingType = setting.SettingValueType,
                    SettingValue = settingValue
                };
            }

            return PartialView("_EditUserSettingView", model);
        }

        [HttpPost]
        public ActionResult EditUserSetting(UserSettingViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.Identity.GetUserId();

                    if (model.Id == 0) //setting for the user doesnt exist yet
                    {
                        var userSetting = new UserSetting()
                        {
                            ApplicationUserID = userId,
                            SettingId = model.SettingId,
                            SettingValue = model.SettingValue
                        };

                        _settingService.CreateUserSetting(userSetting);
                    }
                    else
                    {
                        var setting = _settingService.GetUserSettingById(model.Id);

                        if (setting != null)
                        {
                            setting.SettingValue = model.SettingValue;

                            _settingService.UpdateUserSetting(setting);
                        }
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
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;

                result = new { Success = "false", Message = error };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult UserSettings()
        {
            string userId = User.Identity.GetUserId();

            var settings = Mapper.Map<List<UserSettingViewModel>>(_settingService.AllUserSetting(userId));

            var existingSettingsId = settings.Select(x => x.Id).ToList();

            var defaultSettings = _settingService.AllSettings().Where(x => !existingSettingsId.Contains(x.Id)).ToList();

            foreach (var setting in defaultSettings)
            {
                var userSettingViewModel = new UserSettingViewModel()
                {
                    Id = 0,
                    SettingId = setting.Id,
                    SettingDescription = setting.SettingDescription,
                    SettingType = setting.SettingValueType
                };

                if (setting.SettingValueType == "System.Int32")
                {
                    userSettingViewModel.SettingValue = _defaultNumberOfDays.ToString();
                }
                else
                {
                    userSettingViewModel.SettingValue = string.Empty;
                }

                settings.Add(userSettingViewModel);
            }

            var model = new UserSettingsViewModel()
            {
                UserId = userId,
                Settings = settings
            };

            return View(model);
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

                try
                {
                    newCompany.CreatedDate = DateTime.Now;
                    newCompany.UpdatedDate = newCompany.CreatedDate;

                    newCompany.Address.AddressType = _addressService.GetAddressTypeByName(Enumerations.AddressType.Zameldowania.ToString());
                    newCompany.ApplicationUserID = User.Identity.GetUserId();

                    _userCompanyService.Create(newCompany);
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
    }
}