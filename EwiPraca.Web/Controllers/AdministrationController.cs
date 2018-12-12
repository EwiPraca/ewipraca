using AutoMapper;
using EwiPraca.App_Start.Identity;
using EwiPraca.Constants;
using EwiPraca.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize(Roles = RolesNames.Administrator)]
    public class AdministrationController : Controller
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AdministrationController(ApplicationUserManager applicationUserManager)
        {
            _applicationUserManager = applicationUserManager;
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
    }
}