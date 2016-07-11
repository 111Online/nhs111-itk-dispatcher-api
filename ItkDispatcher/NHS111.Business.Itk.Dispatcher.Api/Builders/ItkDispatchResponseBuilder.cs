
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using NHS111.Domain.Itk.Dispatcher.Models;

namespace NHS111.Business.Itk.Dispatcher.Api.Builders {
    public class ItkDispatchResponseBuilder : IItkDispatchResponseBuilder {
        private const submitEncounterToServiceResponseOverallStatus SUCCESS_RESPONSE =
            submitEncounterToServiceResponseOverallStatus.Successful_call_to_gp_webservice;

        public ItkDispatchResponse Build(SubmitHaSCToServiceResponse submitHaScToServiceResponse) {
            return new ItkDispatchResponse {
                StatusCode =
                    DetermineSuccess(submitHaScToServiceResponse.SubmitEncounterToServiceResponse.OverallStatus),
                Body = submitHaScToServiceResponse.SubmitEncounterToServiceResponse.OverallStatus.ToString()
            };
        }

        // Suggest mapping this an automapper mapping: submitEncounterToServiceResponseOverallStatus -> HttpStatusCode
        private HttpStatusCode DetermineSuccess(submitEncounterToServiceResponseOverallStatus responseStatus) {
            if (responseStatus == SUCCESS_RESPONSE) return HttpStatusCode.OK;
            return HttpStatusCode.InternalServerError;
        }

        public ItkDispatchResponse Build(Exception exception) {
            return new ItkDispatchResponse
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Body = "An error has occured processing the request.",
                Content = new StringContent(JsonConvert.SerializeObject(exception.Message), Encoding.UTF8, "application/json")
            };
        }
    }

    public interface IItkDispatchResponseBuilder {
        ItkDispatchResponse Build(SubmitHaSCToServiceResponse submitHaScToServiceResponse);
        ItkDispatchResponse Build(Exception exception);
    }
}