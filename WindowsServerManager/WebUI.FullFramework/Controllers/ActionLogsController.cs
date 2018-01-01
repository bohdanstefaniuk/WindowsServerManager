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
        private IActionLogger ActionLogger => HttpContext.GetOwinContext().GetUserManager<IActionLogger>();
        // GET: ActionLogs
        public ActionResult Index()
        {
            var actionLogs = ActionLogger.GetActionLogsAsync();
            return View(actionLogs);
        }
    }
}