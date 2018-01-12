using System;
using System.Web.Helpers;
using System.Web.Mvc;

namespace WebUI.FullFramework.Core
{
    public class ActionExceptionAttribute: FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.HttpContext.Request.IsAjaxRequest() || exceptionContext.Exception == null) return;

            if (!exceptionContext.ExceptionHandled)
            {
                NLogger.Log(exceptionContext.Exception);
                exceptionContext.Result = new JsonResult
                {
                    Data = new { success = false, responseText = $"{exceptionContext.Exception.Message}" }
                };
                exceptionContext.ExceptionHandled = true;
            }
        }
    }
}