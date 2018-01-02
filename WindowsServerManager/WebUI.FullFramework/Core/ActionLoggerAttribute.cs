using System;
using System.Web.Mvc;
using BLL.Infrastructure;
using BLL.Services;
using DataAccessLayer.Entities;
using Newtonsoft.Json;
using NLog;

namespace WebUI.FullFramework.Core
{
    public class ActionLoggerAttribute: ActionFilterAttribute
    {
        private ActionLog log;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            log = new ActionLog()
            {
                Id = Guid.NewGuid(),
                Action = filterContext.RouteData.Values["Action"] as string,
                Controller = filterContext.RouteData.Values["Controller"] as string,
                ActionParams = filterContext.ActionParameters == null ? string.Empty : JsonConvert.SerializeObject(filterContext.ActionParameters),
                StartExecution = DateTime.Now,
                HttpMethod = filterContext.HttpContext.Request.HttpMethod,
                Cookies = JsonConvert.SerializeObject(filterContext.HttpContext.Request.Cookies),
                Headers = JsonConvert.SerializeObject(filterContext.HttpContext.Request.Headers),
                OriginalUrl = filterContext.HttpContext.Request.Url?.ToString(),
                Referer = filterContext.HttpContext.Request.UrlReferrer == null ? string.Empty : filterContext.HttpContext.Request.UrlReferrer.ToString(),
                Form = filterContext.HttpContext.Request.Form == null ? string.Empty : JsonConvert.SerializeObject(filterContext.HttpContext.Request.Form),
                Query = filterContext.HttpContext.Request.QueryString?.ToString() ?? string.Empty,
                UserAgent = filterContext.HttpContext.Request.UserAgent,
                UserHost = filterContext.HttpContext.Request.UserHostAddress
            };
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                log.UserName = filterContext.HttpContext.User.Identity.Name;
            base.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            log.FinishExecution = DateTime.Now;
            log.ExecutionTime = (log.FinishExecution - log.StartExecution).TotalMilliseconds;
            ActionLogger.GetInstance().AddLog(log);
        }
    }
}