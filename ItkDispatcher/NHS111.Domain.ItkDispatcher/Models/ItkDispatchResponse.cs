using System.Net.Http;

namespace NHS111.Domain.Itk.Dispatcher.Models
{
    public class ItkDispatchResponse : HttpResponseMessage
    {
        public string Body { get; set; } //this can be changed to a more complex type as required.
    }
}
