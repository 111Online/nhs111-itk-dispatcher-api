﻿namespace NHS111.Business.Itk.Dispatcher.Api.Attributes
{
    using System.Web.Http.Filters;
    using NHS111.Business.Itk.Dispatcher.Api.Logging;
  
    public class LogHandleErrorForApiAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var requestUri = string.Empty;
            if (context.Request.RequestUri != null)
            {
                requestUri = string.Join("/", context.Request.RequestUri.AbsolutePath);
            }

            Log4Net.Error(string.Format("ERROR on {0}:  {1} - {2} - {3}", requestUri, context.Exception.Message, context.Exception.StackTrace, context.Exception.Data));
        }
    }
}
