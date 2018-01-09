using System;
using System.Net.Http;
using System.Text;
using System.Web;
using NLog;

namespace WebUI.FullFramework.Core
{
    public class NLogger
    {
        private static readonly Logger Nlog = LogManager.GetCurrentClassLogger();
        public static void Log(HttpContext context)
        {
            Nlog.Log(LogLevel.Error, context.Error, RequestToString(context.Request));
        }

        public static void Log(Exception exception)
        {
            Nlog.Log(LogLevel.Error, exception, exception.Message);
        }

        public static void Log(Exception exception, HttpContext context)
        {
            Log(context);
            Log(exception);
        }

        private static string RequestToString(HttpRequest request)
        {
            var message = new StringBuilder();
            message.Append("Http method: " + request.HttpMethod);
            message.Append("|Uri: ").Append(request.Url);
            return message.ToString();
        }
    }
}