﻿//namespace WebUI.FullFramework.Core
//{
//    public class NLogExceptionLogger : ExceptionLogger
//    {
//        private static readonly Logger Nlog = LogManager.GetCurrentClassLogger();
//        public override void Log(ExceptionLoggerContext context)
//        {
//            Nlog.LogException(LogLevel.Error, RequestToString(context.Request), context.Exception);
//        }

//        private static string RequestToString(HttpRequestMessage request)
//        {
//            var message = new StringBuilder();
//            if (request.Method != null)
//                message.Append(request.Method);

//            if (request.RequestUri != null)
//                message.Append(" ").Append(request.RequestUri);

//            return message.ToString();
//        }
//    }
//}