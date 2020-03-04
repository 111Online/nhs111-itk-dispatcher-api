
namespace NHS111.Data.Itk.Dispatcher.Repositories {
    using System.Threading;
    using System.Threading.Tasks;

    public interface IItkDispatchRequestRepository {
        Task InsertAsync(string id, string request, CancellationToken cancellationToken);
    }
}