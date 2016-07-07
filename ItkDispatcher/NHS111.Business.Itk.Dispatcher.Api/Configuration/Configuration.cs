using System.Configuration;

namespace NHS111.Business.Itk.Dispatcher.Api.Configuration
{
    public class Configuration : IConfiguration
    {
        public string EsbEndpointUrl
        {
            get { return ConfigurationManager.AppSettings["EsbEndpointUrl"];}
        }
    }
}