namespace NHS111.Business.Itk.Dispatcher.API.Functional.Tests
{
    using System.Net;
    using System.Net.Http;

    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient Get(string username, string password)
        {
            var networkCredential = new NetworkCredential(username, password);

            var httpClientHandler = new HttpClientHandler { Credentials = networkCredential };

            return new HttpClient(httpClientHandler);
        }
    }
}
