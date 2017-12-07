using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Dto;
using BLL.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace WebUI.FullFramework.Controllers
{
    public class IISController : Controller
    {
        private IJsTreeMenuService JsTreeViewMenuService => HttpContext.GetOwinContext().GetUserManager<IJsTreeMenuService>();

        public ActionResult Index()
        {
            return View();
        }

        #region Methods: Get partial Views (Components)

        public PartialViewResult GetFeaturesComponent()
        {
            return PartialView("_FeaturesComponent");
        }

        public PartialViewResult GetConnectionStringsComponent()
        {
            return PartialView("_ConnectionStringsComponent");
        }

        public PartialViewResult GetConfigurationFileComponent()
        {
            return PartialView("_ConfigurationFileComponent");
        }

        #endregion

        #region Methods: Tree View Menu handlers

        /// <summary>
        /// Returns tree view result for creating JsTree menu at View
        /// </summary>
        /// <returns>JsonResult</returns>
        public ActionResult GetIISMenuModel()
        {
            var result = JsTreeViewMenuService.GetTreeMenuData();
            var jsonResult = JsonConvert.SerializeObject(result, Formatting.Indented);
            return Content(jsonResult, "application/json");
        }

        #endregion
    }
}