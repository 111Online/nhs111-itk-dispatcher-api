using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

using Newtonsoft.Json;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using NHS111.Domain.Itk.Dispatcher.Models;
using Address = NHS111.Domain.Itk.Dispatcher.Models.Address;
using PatientDetails = NHS111.Domain.Itk.Dispatcher.Models.PatientDetails;

namespace NHS111.Business.Itk.Dispatcher.Api.Mappings
{
    public class FromItkDispatchRequestToSubmitEncounterToServiceRequest : Profile
    {
        public override string ProfileName
        {
            get { return "FromItkDispatchRequestToSubmitEncounterToServiceRequest"; }
        }

        protected override void Configure()
        {
            CreateMap<Address, ItkDispatcherSOAPService.Address>();

            CreateMap<GpPractice, GPPractice>();

            CreateMap<ServiceDetails, SubmitToServiceDetails>()
                .ForMember(dest => dest.contactDetails, opt => opt.Ignore())
                .ForMember(dest => dest.address, opt => opt.Ignore());

            CreateMap<ItkDispatchRequest, SubmitEncounterToServiceRequest>()
                .ForMember(dest => dest.SendToRepeatCaller, opt => opt.Ignore())
                .ForMember(dest => dest.CaseDetails, opt => opt.MapFrom(src => src.CaseDetails));

            CreateMap<CaseDetails, SubmitToCallQueueDetails>()
                .ForMember(dest => dest.CaseSummary,
                    opt => opt.Condition(src => src.ReportItems != null || src.ConsultationSummaryItems != null))
                .ForMember(dest => dest.CaseSummary, opt => opt.ResolveUsing(ResolveCaseSummary))
                .ForMember(dest => dest.CaseSteps, opt => opt.ResolveUsing(ResolveCaseSteps))
                .ForMember(dest => dest.conditionTitle, opt => opt.MapFrom(src => src.StartingPathwayTitle))
                .ForMember(dest => dest.conditionId, opt => opt.MapFrom(src => src.StartingPathwayId))
                .ForMember(dest => dest.conditionType, opt => opt.MapFrom(src => src.StartingPathwayType))
                .ForMember(dest => dest.UnstructuredData, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.SetVariables)))
                .ForMember(dest => dest.Provider, opt => opt.Ignore())
                .ForMember(dest => dest.Source, opt => opt.Ignore());

            CreateMap<PatientDetails, SubmitPatientService>()
                .ConvertUsing<FromPatientDetailsTSubmitPatientServiceConverter>();
        }

        private static DataInstance[] ResolveCaseSummary(CaseDetails caseDetails)
        {
            if (caseDetails.ConsultationSummaryItems == null && caseDetails.ReportItems == null) return null;

            var items = new List<DataInstance>();
            if (caseDetails.ReportItems != null)
                items.AddRange(caseDetails.ReportItems.Select(i => new DataInstance() { Caption = i.Text, Name = "ReportText", Values = new string[] { i.Text } }));

            if (caseDetails.ConsultationSummaryItems != null)
                items.AddRange(caseDetails.ConsultationSummaryItems.Select(c => new DataInstance() { Caption = c, Name = "DispositionDisplayText", Values = new string[] { c } }));
            
            return items.ToArray();
        }

        private static stepInstance[] ResolveCaseSteps(CaseDetails caseDetails)
        {
            var items = new List<stepInstance>();
            if (caseDetails.CaseSteps != null)
                items.AddRange(caseDetails.CaseSteps.Select(i => new stepInstance() { QuestionId = i.QuestionId, QuestionNo = i.QuestionNo, AnswerOrder = i.AnswerOrder }));

            return items.ToArray();
        }
    }

    public class FromPatientDetailsTSubmitPatientServiceConverter : ITypeConverter<PatientDetails, SubmitPatientService>
    {
        public SubmitPatientService Convert(PatientDetails source, SubmitPatientService destination, ResolutionContext context)
        {
            var submitPatientService = destination ?? new SubmitPatientService();

            submitPatientService.EmailAddress = source.EmailAddress;
            submitPatientService.TelephoneNumber = source.TelephoneNumber;
            submitPatientService.Forename = source.Forename;
            submitPatientService.Surname = source.Surname;

            gender gender = Enum.TryParse(source.Gender, out gender) ? gender : gender.Not_Known;
            submitPatientService.Gender = gender;
            submitPatientService.AgeGroup = source.AgeGroup;

            submitPatientService.DateOfBirth = new DateOfBirth {Item = source.DateOfBirth.ToString("yyyy-MM-dd")};

            submitPatientService.CurrentAddress = context.Mapper.Map<ItkDispatcherSOAPService.Address>(source.CurrentAddress);
            submitPatientService.HomeAddress = context.Mapper.Map<ItkDispatcherSOAPService.Address>(source.HomeAddress);
            submitPatientService.GpPractice = context.Mapper.Map<GPPractice>(source.GpPractice);

            submitPatientService.InformantType = informantType.Self;
            if (source.Informant == null) return submitPatientService;

            informantType informant = Enum.TryParse(source.Informant.Type.ToString(), out informant) ? informant : informantType.NotSpecified;
            submitPatientService.InformantType = informant;

            if (!string.IsNullOrEmpty(source.Informant.Forename) || !string.IsNullOrEmpty(source.Informant.Surname))
                submitPatientService.InformantName = string.Format("{0} {1}", source.Informant.Forename, source.Informant.Surname);

            return submitPatientService;
        }
    }
}