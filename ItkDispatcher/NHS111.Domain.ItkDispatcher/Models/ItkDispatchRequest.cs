namespace NHS111.Domain.Itk.Dispatcher.Models
{
    public class ItkDispatchRequest
    {
        public Authentication Authentication { get; set; }
        public PatientDetails PatientDetails { get; set; }
        public ServiceDetails ServiceDetails { get; set; }
        public CaseDetails CaseDetails { get; set; }
    }
}
