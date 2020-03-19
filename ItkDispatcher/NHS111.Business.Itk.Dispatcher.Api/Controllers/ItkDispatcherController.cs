using System;
using System.Threading.Tasks;
using System.Web.Http;

using log4net;

using Newtonsoft.Json;
using NHS111.Business.Itk.Dispatcher.Api.Builders;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using NHS111.Business.Itk.Dispatcher.Api.Mappings;
using NHS111.Domain.Itk.Dispatcher.Exceptions;
using NHS111.Domain.Itk.Dispatcher.Models;
using NHS111.Domain.Itk.Dispatcher.Services;

namespace NHS111.Business.Itk.Dispatcher.Api.Controllers
{
    using NHS111.Business.Itk.Dispatcher.Api.Attributes;

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
            _logger.Info(string.Format("Request received of JourneyId {0} and external ref {1}", request.CaseDetails.JourneyId, request.CaseDetails.ExternalReference));

            request.CaseDetails.ExternalReference = _patientReferenceService.BuildReference(request.CaseDetails);
            
            var messageExists = _messageService.MessageAlreadyExists(JsonConvert.SerializeObject(request));
            
            if (messageExists)
            {
                _logger.Error(string.Format("Duplicate Case sent of JourneyId {0} and external ref {1}", request.CaseDetails.JourneyId, request.CaseDetails.ExternalReference));
                
                return _itkDispatchResponseBuilder.Build(new DuplicateMessageException(request.CaseDetails.ExternalReference));
            }

            var submitHaSCToService = AutoMapperWebConfiguration.Mapper.Map<ItkDispatchRequest, SubmitHaSCToService>(request);
           
            SubmitHaSCToServiceResponse itkResponse;

            try {
                itkResponse = await _itkDispatcher.SubmitHaSCToServiceAsync(submitHaSCToService);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Send to ESB error for journeyId {0} and external Ref {1}", request.CaseDetails.JourneyId, request.CaseDetails.ExternalReference), ex);

                return _itkDispatchResponseBuilder.Build(ex);
            }

            var response = _itkDispatchResponseBuilder.Build(itkResponse, request.CaseDetails.ExternalReference);

            if (response.IsSuccessStatusCode)
            {
                _messageService.StoreMessage(JsonConvert.SerializeObject(request));
            }

            return response;
        }
    }
}
