using AutoMapper;
using EwiPraca.App_Start.Identity;
using EwiPraca.Attributes;
using EwiPraca.Constants;
using EwiPraca.Models;
using EwiPraca.Services.Interfaces;
using EwiPraca.Services.Services;
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize(Roles = RolesNames.Administrator)]
    [CookieConsent]
    public class AdministrationController : Controller
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private const string gdpr_forget_data_hash = "RODO";
        private readonly IUserCompanyService _userCompanyService;
        private readonly IEmployeeService _employeeService;

        public AdministrationController(ApplicationUserManager applicationUserManager,
            IUserCompanyService userCompanyService,
            IEmployeeService employeeService)
        {
            _applicationUserManager = applicationUserManager;
            _userCompanyService = userCompanyService;
            _employeeService = employeeService;
        }

        public ActionResult Index()
        {
            var users = Mapper.Map<List<UserViewModel>>(_applicationUserManager.Users.ToList());

            return View(users);
        }

        public ActionResult Logs()
        {
            var model = new ErrorLogsViewModel();
            string path = ControllerContext.HttpContext.Server.MapPath("~/logs");
            try
            {
                List<string> files = new List<string>();
                if (Directory.Exists(path))
                {
                    files.AddRange(Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories).ToList());
                }

                foreach (string filePath in files)
                {
                    string text = System.IO.File.ReadAllText(filePath);

                    model.ErrorLogs.Add(new ErrorLogViewModel()
                    {
                        FilePath = filePath,
                        FileContent = text
                    });
                }
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult ShowLogFileView(string filename)
        {
            var model = new ErrorLogViewModel();
            string path = ControllerContext.HttpContext.Server.MapPath("~/logs");
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories).ToList();
                var filepath = files.FirstOrDefault(x => x.Contains(filename));
                model.FileContent = System.IO.File.ReadAllText(filepath);
                model.FilePath = filepath;
            }

            return PartialView("_LogFileModal", model);
        }

        [HttpPost]
        [OutputCache(Duration = 0)]
        public async Task<JsonResult> LockUser(string userId, bool locked)
        {
            var result = new { Success = true, Message = "Success" };

            try
            {
                var email = Encryptor.EncryptionService.EncryptEmail(userId);
                var user = await _applicationUserManager.FindByEmailAsync(email);

                user.IsActive = !locked;

                if (!locked)
                {
                    user.LockoutEndDateUtc = DateTime.UtcNow;
                    user.LastLoginDate = DateTime.UtcNow;
                }
                else
                {
                    user.LockoutEndDateUtc = DateTime.MaxValue;
                }

                var identityResult = await _applicationUserManager.UpdateAsync(user);

                if (identityResult.Succeeded)
                {
                    if (locked)
                    {
                        _applicationUserManager.UpdateSecurityStamp(user.Id);
                    }
                }
                else
                {
                    result = new { Success = false, Message = identityResult.Errors.First() };
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                result = new { Success = false, ex.Message };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [OutputCache(Duration = 0)]
        public async Task<JsonResult> AnonymiseUser(string userId)
        {
            var result = new { Success = true, Message = "Success" };

            try
            {
                var email = Encryptor.EncryptionService.EncryptEmail(userId);
                var user = await _applicationUserManager.FindByEmailAsync(email);

                user.Email = Encryptor.EncryptionService.EncryptEmail("rodo@rodo.pl");
                user.FirstName = gdpr_forget_data_hash;
                user.Surname = gdpr_forget_data_hash;
                user.UserName = gdpr_forget_data_hash;
                user.IsActive = false;

                var userCompanies = _userCompanyService.GetUserCompanies(user.Id);
               
                foreach(var company in userCompanies)
                {
                    company.CompanyName = gdpr_forget_data_hash;
                    company.Address.PlaceNumber = gdpr_forget_data_hash;
                    company.Address.StreetName = gdpr_forget_data_hash;
                    company.Address.ZIPCode = gdpr_forget_data_hash;
                    company.Address.StreetNumber = gdpr_forget_data_hash;

                    _userCompanyService.Update(company);

                    foreach(var employee in company.Employees)
                    {
                        employee.FirstName = gdpr_forget_data_hash;
                        employee.Surname = gdpr_forget_data_hash;
                        employee.PESEL = gdpr_forget_data_hash;
                        employee.Address.PlaceNumber = gdpr_forget_data_hash;
                        employee.Address.StreetName = gdpr_forget_data_hash;
                        employee.Address.ZIPCode = gdpr_forget_data_hash;
                        employee.Address.StreetNumber = gdpr_forget_data_hash;

                        _employeeService.Update(employee);
                    }

                }

                _applicationUserManager.Update(user);

            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                result = new { Success = false, ex.Message };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}