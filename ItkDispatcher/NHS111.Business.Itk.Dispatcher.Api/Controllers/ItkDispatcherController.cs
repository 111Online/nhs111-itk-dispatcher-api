using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using NHS111.Business.Itk.Dispatcher.Api.Builders;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using NHS111.Domain.Itk.Dispatcher.Models;

namespace NHS111.Business.Itk.Dispatcher.Api.Controllers
{
    public class ItkDispatcherController : ApiController
    {
        private readonly MessageEngine _itkDispatcher;
        private readonly IMappingEngine _mappingEngine;
        private readonly IItkDispatchResponseBuilder _itkDispatchResponseBuilder;

        public ItkDispatcherController(MessageEngine itkDispatcher, IMappingEngine mappingEngine, IItkDispatchResponseBuilder itkDispatchResponseBuilder)
        {
            _itkDispatcher = itkDispatcher;
            _mappingEngine = mappingEngine;
            _itkDispatchResponseBuilder = itkDispatchResponseBuilder;
        }

        [HttpPost]
        [Route("SendItkMessage")]
        public async Task<ItkDispatchResponse> SendItkMessage(ItkDispatchRequest request)
        {
            BypassCertificateError();
            var submitHaSCToService = _mappingEngine.Mapper.Map<ItkDispatchRequest, SubmitHaSCToService>(request);
            SubmitHaSCToServiceResponse response = null;
            try {
                response = await _itkDispatcher.SubmitHaSCToServiceAsync(submitHaSCToService);
            }
            catch (Exception ex) {
                return _itkDispatchResponseBuilder.Build(ex);
            }
            return _itkDispatchResponseBuilder.Build(response);
        }

        /// <summary>
        /// Temorary sssl cert validation bypass until ESB hosting has domain name
        /// </summary>
        private static void BypassCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback +=

                delegate(
                    Object sender1,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }
    }
}
