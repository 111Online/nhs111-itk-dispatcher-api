namespace NHS111.Business.Itk.Dispatcher.API.Functional.Tests
{
    using System.Net.Http;

    public interface IHttpClientFactory
    {
        HttpClient Get(string username, string password);
    }
}