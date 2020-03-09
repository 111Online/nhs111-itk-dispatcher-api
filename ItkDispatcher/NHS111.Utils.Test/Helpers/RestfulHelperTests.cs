using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NHS111.Utils.Helpers;
using NUnit.Framework;

namespace NHS111.Utils.Test.Helpers
{
    [TestFixture()]
    public class RestfulHelperTests
    {
        private string _mockUri = "http://testuri.com";

        [Test()]
        public void PostAsync_sends_clean_httpHeaders_Test()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddFakeResponse(new Uri(_mockUri), new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = new HttpClient(fakeResponseHandler);
            var request = GenerateTestRequest();

            request.Headers.Add("TestHeader", "testheader content");
            RestfulHelper restfulHelper = new RestfulHelper(httpClient);

            var response = restfulHelper.PostAsync(_mockUri, request);

            Assert.IsTrue(response.Result.IsSuccessStatusCode);
            Assert.IsEmpty(fakeResponseHandler.SentRequest.Headers);
        }

        [Test()]
        public void PostAsync_withHeaders_sends_httpHeaders_Test()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddFakeResponse(new Uri(_mockUri), new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = new HttpClient(fakeResponseHandler);

            var request = GenerateTestRequest();
            RestfulHelper restfulHelper = new RestfulHelper(httpClient);

            var response = restfulHelper.PostAsync(_mockUri, request, new Dictionary<string, string>(){{"testHeader", "Test header content"}});

            Assert.IsTrue(response.Result.IsSuccessStatusCode);
            Assert.IsNotEmpty(fakeResponseHandler.SentRequest.Headers);
            Assert.IsTrue(fakeResponseHandler.SentRequest.Headers.Any(h => h.Key == "testHeader"));
        }

        private static HttpRequestMessage GenerateTestRequest()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("{'Test':'Some test content'}", Encoding.UTF8, "application/json")
            };

            return request;
        }


        public class FakeResponseHandler : DelegatingHandler
        {
            private readonly Dictionary<Uri, HttpResponseMessage> _FakeResponses = new Dictionary<Uri, HttpResponseMessage>();

            public void AddFakeResponse(Uri uri, HttpResponseMessage responseMessage)
            {
                _FakeResponses.Add(uri, responseMessage);
            }

            private HttpRequestMessage _requestSent; 
            protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                _requestSent = request;
                if (_FakeResponses.ContainsKey(request.RequestUri))
                {
                    return _FakeResponses[request.RequestUri];
                }
                     return new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request };
            }

            public HttpRequestMessage SentRequest
            {
                get{return _requestSent;}  
            }
        }
    }
}
