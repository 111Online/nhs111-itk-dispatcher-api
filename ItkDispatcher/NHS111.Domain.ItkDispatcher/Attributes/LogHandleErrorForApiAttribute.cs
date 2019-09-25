using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Utils.Logging;

namespace NHS111.Domain.Itk.Dispatcher.Attributes
{

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
