using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interfaces;
using Microsoft.AspNet.Identity.Owin;

namespace WebUI.FullFramework.Controllers
{
    public class SettingsController : Controller
    {
        private ISettingsService SettingsService => HttpContext.GetOwinContext().GetUserManager<ISettingsService>();

        // GET: Settings
        public ActionResult Index()
        {
            var settings = SettingsService.GetSettings();
            return View(settings);
        }
    }
}