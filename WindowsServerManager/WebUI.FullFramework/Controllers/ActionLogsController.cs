using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interfaces;
using Microsoft.AspNet.Identity.Owin;

namespace WebUI.FullFramework.Controllers
{
    public class ActionLogsController : Controller
    {
        private IActionLogsService ActionLogsService => HttpContext.GetOwinContext().GetUserManager<IActionLogsService>();
        // GET: ActionLogs
        public ActionResult Index(int page = 1)
        {
            var actionLogs = ActionLogsService.GetPagedActionLogs(page, 50);
            return View(actionLogs);
        }
    }
}