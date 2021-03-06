﻿using AutoMapper;
using EwiPraca.App_Start.Identity;
using EwiPraca.Attributes;
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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    [CookieConsent]
    public class ManageController : Controller
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly ISettingService _settingService;
        private readonly AddressService _addressService;
        private readonly IUserCompanyService _userCompanyService;
        private readonly ISharedLinkService _sharedFileService;
        private readonly IUserFileService _userFileService;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly int _defaultNumberOfDays = SettingsHandler.DaysBeforeIntervalReminder;
        private readonly string filePath = "MediaFiles";

        public ManageController(ApplicationUserManager applicationUserManager,
            IUserCompanyService userCompanyService,
            ISettingService settingService,
            IUserFileService userFileService,
            ISharedLinkService sharedFileService,
            AddressService addressService)
        {
            _applicationUserManager = applicationUserManager;
            _userCompanyService = userCompanyService;
            _settingService = settingService;
            _addressService = addressService;
            _userFileService = userFileService;
            _sharedFileService = sharedFileService;
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
        public ActionResult ChangePasswordView()
        {
            return PartialView("ChangePasswordViewModal", new ChangePasswordViewModel());
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
        [ValidateAntiForgeryToken]
        public ActionResult ChangeUserPassword(ChangePasswordViewModel model)
        {
            var result = new { Success = "true", Message = "Success" };

            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("UserId cannot be empty!");
                }

                var user = _applicationUserManager.FindById(userId);

                if (user == null)
                {
                    throw new Exception("User not found!");
                }

                var loggedUser = _applicationUserManager.Find(user.Email, model.CurrentPassword);

                if (loggedUser == null)
                {
                    return Json(new { Success = "false", Message = "Stare hasło jest nieprawidłowe." }, JsonRequestBehavior.AllowGet);
                }

                var resetToken = _applicationUserManager.GeneratePasswordResetToken(user.Id);
                var res = _applicationUserManager.ResetPassword(user.Id, resetToken, model.Password);

                if (res.Succeeded)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string message = res.Errors.FirstOrDefault();
                    logger.Warn(string.Format("{0} {1} {2}", message, "UserId", userId));
                    return Json(new { Success = "false", Message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;

                result = new { Success = "false", Message = error };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
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

            if (!string.IsNullOrEmpty(userId))
            {
                RedirectToAction("Index", "Home");
            }

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

            var user = _applicationUserManager.FindById(userId);

            var model = new UserSettingsViewModel()
            {
                UserId = userId,
                Settings = settings,
                TwoFactorEnabled = user.TwoFactorEnabled
            };

            return View(model);
        }


        [HttpGet]
        public ActionResult UserDocuments(string currentFolderGuid = "")
        {
            var userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                RedirectToAction("Index", "Home");
            }

            var model = new UserDocumentsViewModel() { CurrentFolderGuid = currentFolderGuid };

            if (!string.IsNullOrEmpty(currentFolderGuid))
            {
                var parentFolder = _userFileService.GetByGuid(currentFolderGuid);
                model.ParentFolderGuid = parentFolder.ParentFile?.FileGuid;
            }

            if (string.IsNullOrEmpty(currentFolderGuid))
            {
                model.Files = GetUserRootFiles(userId);
            }
            else
            {
                model.Files = GetUserFolderGuidFiles(currentFolderGuid);
            }

            return View(model);
        }

        private List<UserFileViewModel> GetUserFolderGuidFiles(string currentFolderGuid)
        {
            var userId = User.Identity.GetUserId();

            var files = _userFileService.GetFilesByFolderGuid(userId, currentFolderGuid);

            List<UserFileViewModel> filesViewModel = new List<UserFileViewModel>();

            foreach (var file in files)
            {
                filesViewModel.Add(new UserFileViewModel()
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    FolderGuid = file.ContentType == null ? file.FileGuid : null,
                    FileGuid = file.FileGuid
                });
            }

            return filesViewModel.OrderBy(o => o.ContentType != null).ThenByDescending(o => o.ContentType).ToList();
        }

        private List<UserFileViewModel> GetUserRootFiles(string userId)
        {
            var files = _userFileService.GetRootFilesForUser(userId);

            List<UserFileViewModel> filesViewModel = new List<UserFileViewModel>();

            foreach (var file in files)
            {
                filesViewModel.Add(new UserFileViewModel()
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    FolderGuid = file.ContentType == null ? file.FileGuid : null,
                    FileGuid = file.FileGuid
                });
            }

            return filesViewModel.OrderBy(o => o.ContentType != null).ThenByDescending(o => o.ContentType).ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUserFolder(string CurrentFolderGuid, string folderName)
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                RedirectToAction("Index", "Home");
            }

            try
            {
                var now = DateTime.Now;

                var folder = new UserFile()
                {
                    CreatedDate = now,
                    UpdatedDate = now,
                    ApplicationUserID = userId,
                    FileName = folderName,
                    FileGuid = Guid.NewGuid().ToString()
                };

                if (!string.IsNullOrEmpty(CurrentFolderGuid)) //rootfolder
                {
                    folder.ParentFileId = _userFileService.GetByGuid(CurrentFolderGuid).Id;
                }

                _userFileService.Create(folder);

            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
            }

            return RedirectToAction("UserDocuments", new UserDocumentsViewModel() { CurrentFolderGuid = CurrentFolderGuid, Files = GetUserRootFiles(userId) });
        }

        [HttpPost]
        public ActionResult UploadUserFile(string folderGuid)
        {
            foreach (string item in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
                if (file.ContentLength == 0)
                    continue;
                if (file.ContentLength > 0)
                {
                    try
                    {
                        var userId = User.Identity.GetUserId();

                        var newFolderForFileName = Guid.NewGuid().ToString();
                        var now = DateTime.Now;
                        string path = string.Format("{0}/{1}", filePath, newFolderForFileName);

                        Directory.CreateDirectory(path);

                        path = path + "/" + file.FileName;

                        file.SaveAs(path);

                        var userFile = new UserFile()
                        {
                            ApplicationUserID = userId,
                            ContentType = file.ContentType,
                            FileGuid = Guid.NewGuid().ToString(),
                            ParentFileId = _userFileService.GetByGuid(folderGuid)?.Id,
                            CreatedDate = now,
                            UpdatedDate = now,
                            FileName = path,
                        };

                        _userFileService.Create(userFile);
                    }
                    catch (Exception e)
                    {
                        throw e;
                        //logger.Error(e, e.Message);
                    }
                }
            }

            return Json("");
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

        [HttpPost]
        public JsonResult DeleteUserFile(string guid)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                var file = _userFileService.GetByGuid(guid);

                if (file != null)
                {
                    _userFileService.Delete(file);
                }
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                result = new { Success = "false", Message = WebResources.ErrorMessage };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PostUserDataFile(string guid)
        {
            string handle = Guid.NewGuid().ToString();
            var ewiFile = _userFileService.GetByGuid(guid);

            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream file = new FileStream(ewiFile.FileName, FileMode.Open, FileAccess.Read))
                {
                    file.CopyTo(ms);
                }

                ms.Position = 0;
                TempData[handle] = ms.ToArray();
            }

            string[] path = ewiFile.FileName.Split('/');

            var result = new { FileGuid = handle, FileName = path[path.Length - 1], ContentType = ewiFile.ContentType };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DownloadUserFile(string fileGuid, string fileName, string contentType)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                TempData[fileGuid] = null;
                return File(data, contentType, fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult GenerateSharedFileLink(string currentFileGuid)
        {
            var result = new { Success = "true", Message = "", Url = "" };

            string guid = Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(currentFileGuid))
            {
                throw new Exception("Brak takiego pliku!");
            }

            try
            {
                SharedFileLink sharedLink = new SharedFileLink()
                {
                    CreatedDate = DateTime.Now,
                    FileGuid = currentFileGuid,
                    GuidLink = guid
                };

                _sharedFileService.Create(sharedLink);

                string url = Url.Action("GetSharedFile", "Manage", new { guid = guid }, Request.Url.Scheme);

                result = new { Success = "true", Message = "", Url = url };
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                result = new { Success = "false", Message = WebResources.ErrorMessage, Url = "" };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSharedFile(string guid)
        {
            string fileLink = _sharedFileService.GetByGuid(guid)?.FileGuid;

            if (string.IsNullOrEmpty(fileLink))
            {
                throw new Exception("Link nie istnieje!");
            }

            UserFile file = _userFileService.GetByGuid(fileLink);

            if (file == null)
            {
                throw new Exception("Plik nie istnieje!");
            }

            if (file.ContentType == null)
            {
                return null;
            }
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(file.FileName, FileMode.Open, FileAccess.Read))
                    {
                        fs.CopyTo(ms);
                    }

                    ms.Position = 0;
                    string[] path = file.FileName.Split('/');
                    return File(ms.ToArray(), file.ContentType, path[path.Length - 1]);
                }

            }
        }
    }
}