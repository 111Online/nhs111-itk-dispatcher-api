using AutoMapper;
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
            CreateMap<GpPractice, ItkDispatcherSOAPService.GPPractice>();
            CreateMap<ServiceDetails, ItkDispatcherSOAPService.SubmitToServiceDetails>();

            CreateMap<ItkDispatchRequest, SubmitEncounterToServiceRequest>();
            CreateMap<CaseDetails, SubmitToCallQueueDetails>();
            CreateMap<PatientDetails, SubmitPatientService>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => new DateOfBirth() {Item = src.DateOfBirth.ToString("yyyy-MM-dd")}))
                .ForMember(src => src.InformantType, opt => opt.UseValue(informantType.Self))
                .ForMember(src => src.CurrentAddress,
                    opt =>
                        opt.MapFrom(
                            src => new ItkDispatcherSOAPService.Address() {PostalCode = src.CurrentLocationPostcode}));



        }
    }
}