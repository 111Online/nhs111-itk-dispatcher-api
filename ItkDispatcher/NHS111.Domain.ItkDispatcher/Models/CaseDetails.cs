using System.Collections.Generic;

namespace NHS111.Domain.Itk.Dispatcher.Models
{
    public class CaseDetails
    {
        public string ExternalReference { get; set; }
        public string Source { get; set; }
        public string DispositionCode { get; set; }
        public string DispositionName { get; set; }
        public List<string> ReportItems { get; set; }
        public List<string> ConsultationSummaryItems { get; set; }
        public List<StepItem> CaseSteps { get; set; }
    }
}
