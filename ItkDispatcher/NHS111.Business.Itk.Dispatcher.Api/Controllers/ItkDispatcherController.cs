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
using log4net;
using log4net.Core;
using Newtonsoft.Json;
using NHS111.Business.Itk.Dispatcher.Api.Builders;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using NHS111.Business.Itk.Dispatcher.Api.Mappings;
using NHS111.Domain.Itk.Dispatcher.Exceptions;
using NHS111.Domain.Itk.Dispatcher.Models;
using NHS111.Domain.Itk.Dispatcher.Services;
using NHS111.Utils.Attributes;

namespace NHS111.Business.Itk.Dispatcher.Api.Controllers
{
    [LogHandleErrorForApi]
    public class ItkDispatcherController : ApiController
    {
        private readonly MessageEngine _itkDispatcher;
        private readonly IItkDispatchResponseBuilder _itkDispatchResponseBuilder;
        private readonly IMessageService _messageService;
        private readonly IPatientReferenceService _patientReferenceService;
        private readonly ILog _logger;

        public ItkDispatcherController(MessageEngine itkDispatcher, IItkDispatchResponseBuilder itkDispatchResponseBuilder, IMessageService messageService, IPatientReferenceService patientReferenceService, ILog logger)
        {
            _itkDispatcher = itkDispatcher;
            _itkDispatchResponseBuilder = itkDispatchResponseBuilder;
            _messageService = messageService;
            _patientReferenceService = patientReferenceService;
            _logger = logger;
        }

        [HttpPost]
        [Route("SendItkMessage")]
        public async Task<ItkDispatchResponse> SendItkMessage(ItkDispatchRequest request)
        {
            _logger.Info(string.Format("Request recieved of JourneyId {0} and external ref {1}", request.CaseDetails.JourneyId, request.CaseDetails.ExternalReference));
            request.CaseDetails.ExternalReference = _patientReferenceService.BuildReference(request.CaseDetails);
            var messageExists = _messageService.MessageAlreadyExists(request.CaseDetails.JourneyId, JsonConvert.SerializeObject(request));
            if (messageExists)
            {
                _logger.Error(string.Format("Duplicate Case sent of JourneyId {0} and external ref {1}", request.CaseDetails.JourneyId, request.CaseDetails.ExternalReference));
                return _itkDispatchResponseBuilder.Build(new DuplicateMessageException(request.CaseDetails.ExternalReference));
            }

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
                _logger.Error(String.Format("Send to ESB error for journeyId {0} and external Ref {1}", request.CaseDetails.JourneyId, request.CaseDetails.ExternalReference), ex);
                return _itkDispatchResponseBuilder.Build(ex);
            }

            var response = _itkDispatchResponseBuilder.Build(itkResponse, request.CaseDetails.ExternalReference);
            if(response.IsSuccessStatusCode)
                _messageService.StoreMessage(request.CaseDetails.JourneyId, JsonConvert.SerializeObject(request));

            return response;
        }
    }
}
