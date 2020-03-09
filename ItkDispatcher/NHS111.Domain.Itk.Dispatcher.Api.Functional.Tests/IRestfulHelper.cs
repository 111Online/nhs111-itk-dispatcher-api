namespace NHS111.Business.Itk.Dispatcher.API.Functional.Tests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IRestfulHelper
    {
        Task<HttpResponseMessage> GetResponseAsync(string url);

        Task<HttpResponseMessage> GetResponseAsync(string url, string username, string password);

        Task<string> GetAsync(string url);

        Task<string> GetAsync(string url, string credentials);

        Task<HttpResponseMessage> PostAsync(string url, HttpRequestMessage request);

        Task<HttpResponseMessage> PostAsync(string url, HttpRequestMessage request, Dictionary<string, string> headers);
    }
}