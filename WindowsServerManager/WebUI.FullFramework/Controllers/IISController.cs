using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Dto;
using BLL.Enums;
using BLL.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using WebUI.FullFramework.Enums;

namespace WebUI.FullFramework.Controllers
{
    public class IISController : Controller
    {
        private IJsTreeMenuService JsTreeViewMenuService => HttpContext.GetOwinContext().GetUserManager<IJsTreeMenuService>();

        public ActionResult Index(string applicationPath = null, 
            IISSiteType siteType = IISSiteType.Default, 
            IISViewActionType viewActionType = IISViewActionType.InformationComponent)
        {
            ViewBag.Name = applicationPath;
            ViewBag.ActionViewType = viewActionType;
            return View();
        }

        #region Methods: Get partial Views (Components)

        public PartialViewResult GetActionComponentByType(IISViewActionType actionType)
        {
            switch (actionType)
            {
                case IISViewActionType.InformationComponent:
                    return GetInformationComponent();
                case IISViewActionType.ConnectionStringsComponent:
                    return GetConnectionStringsComponent();
                case IISViewActionType.FeaturesComponent:
                    return GetFeaturesComponent();
                case IISViewActionType.ConfigFileComponent:
                    return GetConfigurationFileComponent();
                default:
                    return null;
            }
        }

        [ChildActionOnly]
        public PartialViewResult GetFeaturesComponent()
        {
            return PartialView("_FeaturesComponent");
        }

        [ChildActionOnly]
        public PartialViewResult GetConnectionStringsComponent()
        {
            return PartialView("_ConnectionStringsComponent");
        }

        [ChildActionOnly]
        public PartialViewResult GetConfigurationFileComponent()
        {
            return PartialView("_ConfigurationFileComponent");
        }

        [ChildActionOnly]
        public PartialViewResult GetInformationComponent()
        {
            return PartialView("_InformationComponent");
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