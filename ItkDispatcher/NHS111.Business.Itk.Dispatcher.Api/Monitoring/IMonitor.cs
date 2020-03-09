using System.Threading.Tasks;

namespace NHS111.Business.Itk.Dispatcher.Api.Monitoring
{
    public interface IMonitor
    {
        string Ping();

        string Metrics();

        Task<bool> Health();

        string Version();
    }
}
