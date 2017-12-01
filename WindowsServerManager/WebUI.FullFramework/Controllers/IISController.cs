using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.FullFramework.Controllers
{
    public class IISController : Controller
    {
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

        #region Methods: Private

        private void GetIISMenuModel()
        {
            
        }

        #endregion
    }
}