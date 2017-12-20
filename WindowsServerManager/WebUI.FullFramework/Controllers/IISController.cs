using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using BLL.Dto;
using BLL.Enums;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNet.Identity.Owin;
using MssqlManager.Dto;
using Newtonsoft.Json;
using WebUI.FullFramework.Enums;

namespace WebUI.FullFramework.Controllers
{
    public class IISController : Controller
    {
        private IFeatureService FeatureService => HttpContext.GetOwinContext().GetUserManager<IFeatureService>();
        private IConnectionStringsService ConnectionStringsService => HttpContext.GetOwinContext().GetUserManager<IConnectionStringsService>();
        private IApplicationPoolService ApplicationPoolService => HttpContext.GetOwinContext().GetUserManager<IApplicationPoolService>();

        public ActionResult Index(string applicationPath = null, 
            IISSiteType siteType = IISSiteType.Default, 
            IISViewActionType viewActionType = IISViewActionType.InformationComponent)
        {
            string db = null;
            if (!string.IsNullOrEmpty(applicationPath))
            {
                switch (siteType)
                {
                    case IISSiteType.Application:
                        db = ConnectionStringsService.GetMssqlDb(applicationPath, false);
                        break;
                    case IISSiteType.Site:
                        db = ConnectionStringsService.GetMssqlDb(applicationPath, true);
                        break;
                }
                
            }

            ViewBag.Name = applicationPath;
            ViewBag.SiteType = siteType;
            ViewBag.ActionViewType = viewActionType;
            ViewBag.Database = db;

            if (!string.IsNullOrEmpty(db))
            {
                ViewBag.IsFeatureTableExist = FeatureService.GetFeatureTableExist(db).GetAwaiter().GetResult();
            }
            else
            {
                ViewBag.IsFeatureTableExist = false;
            }

            return View();
        }

        #region Methods: Get partial Views (Components)

        public PartialViewResult GetActionComponentByType(IISViewActionType actionType)
        {
            switch (actionType)
            {
                //case IISViewActionType.InformationComponent:
                //    //return GetInformationComponent();
                //case IISViewActionType.ConnectionStringsComponent:
                //    return GetConnectionStringsComponent();
                //case IISViewActionType.FeaturesComponent:
                //    return GetFeaturesComponent();
                //case IISViewActionType.ConfigFileComponent:
                //    return GetConfigurationFileComponent();
                default:
                    return null;
            }
        }

        [ChildActionOnly]
        public PartialViewResult GetFeaturesComponent(string db)
        {
            //TODO get db name from iis instance
            var features = FeatureService.GetFeatures(db).GetAwaiter().GetResult();
            return PartialView("_FeaturesComponent", features);
        }

        [ChildActionOnly]
        public PartialViewResult GetConnectionStringsComponent(string db)
        {
            return PartialView("_ConnectionStringsComponent");
        }

        [ChildActionOnly]
        public PartialViewResult GetConfigurationFileComponent(string db)
        {
            return PartialView("_ConfigurationFileComponent");
        }

        [ChildActionOnly]
        public PartialViewResult GetInformationComponent(string path, IISSiteType siteType)
        {
            SiteInformation information;
            using (var infoService = new SiteInformationService())
            {
                information = infoService.GetInformationBySiteType(path, siteType);
            }
            return PartialView("_InformationComponent", information);
        }

        #endregion

        [HttpPost]
        public JsonResult SaveFeatures(string db, List<FeatureDto> features)
        {
            //TODO get db name from iis instance
            try
            {
                FeatureService.UpdateFeatures(features, db).GetAwaiter();
            }
            catch (Exception e)
            {
                return Json(new {success = false, responseText = $"{e.Message}"}, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = $"Success" }, JsonRequestBehavior.AllowGet);
        }
    }
}