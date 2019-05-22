using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Domain.Itk.Dispatcher.Models;

namespace NHS111.Domain.Itk.Dispatcher.Services
{
    public class PatientReferenceService : IPatientReferenceService
    {
        private const string REF_PREFIX = "111-ONLINE-";
        public string BuildReference(CaseDetails caseDetails)
        {
            if(String.IsNullOrEmpty(caseDetails.JourneyId)) throw new ArgumentException("caseDetails does not contain a journeyId");
            var partJourneyId = caseDetails.JourneyId.Substring(0,5).ToUpper();
            return String.Format("{0}{1}", REF_PREFIX, partJourneyId);
        }
    }

    public interface IPatientReferenceService
    {
        string BuildReference(CaseDetails caseDetails);
    }
}
