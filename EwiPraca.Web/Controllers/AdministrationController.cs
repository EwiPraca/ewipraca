using AutoMapper;
using EwiPraca.App_Start.Identity;
using EwiPraca.Constants;
using EwiPraca.Models;
using NLog;
using System.Collections.Generic;
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
    }
}