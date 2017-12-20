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

            if (!string.IsNullOrEmpty(applicationPath))
            {
                SiteInformation information;
                // TODO Extract interface for DI
                using (var infoService = new SiteInformationService())
                {
                    information = infoService.GetInformationBySiteType(applicationPath, siteType);
                }

                ViewBag.ApplicationPoolName = information.ApplicationPoolName;
                ViewBag.IsPoolStoppingOrStopped =
                    ApplicationPoolService.IsPoolStoppingOrStopped(information.ApplicationPoolName);
                ViewBag.IsPoolStartingOrStarted =
                    ApplicationPoolService.IsPoolStartingOrStarted(information.ApplicationPoolName);
            }
            
            return View();
        }

        #region Methods: Get partial Views (Components)

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
            // TODO Extract interface for DI
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

        [HttpPost]
        public JsonResult StopApplicationPool(string poolName)
        {
            bool isPoolStopedOrStoping;
            try
            {
                isPoolStopedOrStoping = ApplicationPoolService.StopPoolByName(poolName);
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = $"{e.Message}" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = $"Success", poolStatus = isPoolStopedOrStoping }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StartApplicationPool(string poolName)
        {
            bool isPoolStartedOrStarting;
            try
            {
                isPoolStartedOrStarting = ApplicationPoolService.StartPoolByName(poolName);
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = $"{e.Message}" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = $"Success", poolStatus = isPoolStartedOrStarting }, JsonRequestBehavior.AllowGet);
        }
    }
}