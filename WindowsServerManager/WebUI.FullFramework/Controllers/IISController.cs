using System;
using System.Web;
using System.Web.Mvc;
using BLL.Dto;
using BLL.Enums;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNet.Identity.Owin;
using WebUI.FullFramework.Enums;
using WebUI.FullFramework.Models;

namespace WebUI.FullFramework.Controllers
{
    public class IISController : Controller
    {
        private IFeatureService FeatureService => HttpContext.GetOwinContext().GetUserManager<IFeatureService>();
        private IConnectionStringsService ConnectionStringsService => HttpContext.GetOwinContext().GetUserManager<IConnectionStringsService>();
        private IApplicationPoolService ApplicationPoolService => HttpContext.GetOwinContext().GetUserManager<IApplicationPoolService>();
        private IRedisService RedisService => HttpContext.GetOwinContext().GetUserManager<IRedisService>();

        // TODO Change for view model
        public ActionResult Index(
            string applicationPath = null,
            string siteName = null,
            IISSiteType siteType = IISSiteType.Default, 
            IISViewActionType viewActionType = IISViewActionType.InformationComponent)
        {
            string db = null;
            string redisDb = null;
            if (!string.IsNullOrEmpty(applicationPath))
            {
                switch (siteType)
                {
                    case IISSiteType.Application:
                        db = ConnectionStringsService.GetMssqlDb(applicationPath, false);
                        redisDb = ConnectionStringsService.GetRedisDb(applicationPath, false);
                        break;
                    case IISSiteType.Site:
                        db = ConnectionStringsService.GetMssqlDb(applicationPath, true);
                        redisDb = ConnectionStringsService.GetRedisDb(applicationPath, true);
                        break;
                }
                
            }

            // TODO Move into view model
            // TODO Change and add SiteName
            ViewBag.Name = applicationPath;
            ViewBag.SiteType = siteType;
            ViewBag.ActionViewType = viewActionType;
            ViewBag.Database = db;
            ViewBag.RedisDb = redisDb;

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

        #region Methods: Partial Views (Components)

        [ChildActionOnly]
        public PartialViewResult GetFeaturesComponent(string db, int redisDb)
        {
            ViewBag.RedisDb = redisDb;
            ViewBag.Database = db;
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

        #region Methods: API 

        [HttpPost]
        public JsonResult SaveFeatures(FeaturesComponentUpdateModel featuresUpdateModel)
        {
            try
            {
                FeatureService.UpdateFeatures(featuresUpdateModel.Features, featuresUpdateModel.Db, featuresUpdateModel.RedisDb).GetAwaiter();
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = $"{e.Message}" }, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult RecycleApplicationPool(string poolName)
        {
            bool isPoolStartedOrStarting;
            try
            {
                isPoolStartedOrStarting = ApplicationPoolService.RecyclePoolByName(poolName);
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = $"{e.Message}" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = $"Success", poolStatus = isPoolStartedOrStarting }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FlushRedisDb(int redisDb)
        {
            try
            {
                RedisService.FlushDatabaseAsync(redisDb).GetAwaiter();
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = $"{e.Message}" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = $"Success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteApplicaiton(string name, ApplicationDeleteDepth deleteDepth, IISSiteType siteType)
        {
            try
            {
                ApplicationPoolService.DeleteApplication(name, deleteDepth, siteType);
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = $"{e.Message}" }, JsonRequestBehavior.AllowGet);
            }

            var redirectUrl = Url.Action("Index", "IIS");
            return Json(new { success = true, responseText = $"Success", redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}