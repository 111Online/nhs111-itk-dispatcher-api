using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Business.Itk.Dispatcher.Api.Builders;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using NHS111.Business.Itk.Dispatcher.Api.Mappings;
using NHS111.Domain.Itk.Dispatcher.Exceptions;
using NHS111.Domain.Itk.Dispatcher.Models;
using NHS111.Domain.Itk.Dispatcher.Services;

namespace NHS111.Business.Itk.Dispatcher.Api.Controllers
{
    public class ItkDispatcherController : ApiController
    {
        private readonly MessageEngine _itkDispatcher;
        private readonly IItkDispatchResponseBuilder _itkDispatchResponseBuilder;
        private readonly IMessageService _messageService;

        public ItkDispatcherController(MessageEngine itkDispatcher, IItkDispatchResponseBuilder itkDispatchResponseBuilder, IMessageService messageService)
        {
            _itkDispatcher = itkDispatcher;
            _itkDispatchResponseBuilder = itkDispatchResponseBuilder;
            _messageService = messageService;
        }

        [HttpPost]
        [Route("SendItkMessage")]
        public async Task<ItkDispatchResponse> SendItkMessage(ItkDispatchRequest request)
        {
            var messageExists = _messageService.MessageAlreadyExists(request.CaseDetails.ExternalReference, JsonConvert.SerializeObject(request));
            if (messageExists) return _itkDispatchResponseBuilder.Build(new DuplicateMessageException("This message has already been successfully submitted"));

            BypassCertificateError();
            var submitHaSCToService = AutoMapperWebConfiguration.Mapper.Map<ItkDispatchRequest, SubmitHaSCToService>(request);

#if DEBUG
            var xsSubmit = new XmlSerializer(typeof(SubmitHaSCToService));
            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, submitHaSCToService);
                    var xml = sww.ToString();
                }
            }
#endif
            SubmitHaSCToServiceResponse itkResponse = null;
            try {
                itkResponse = await _itkDispatcher.SubmitHaSCToServiceAsync(submitHaSCToService);
            }
            catch (Exception ex) {
                return _itkDispatchResponseBuilder.Build(ex);
            }

            var response = _itkDispatchResponseBuilder.Build(itkResponse);
            if(response.IsSuccessStatusCode)
                _messageService.StoreMessage(request.CaseDetails.ExternalReference, JsonConvert.SerializeObject(request));

            return response;
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
