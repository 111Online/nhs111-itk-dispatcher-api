using AutoMapper;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using NHS111.Domain.Itk.Dispatcher.Models;
using Authentication = NHS111.Domain.Itk.Dispatcher.Models.Authentication;

namespace NHS111.Business.Itk.Dispatcher.Api.Mappings
{
    public class FromITKDispatchRequestToSubmitHaSCToService : Profile
    {
        public override string ProfileName
        {
            get { return "FromITKDispatchRequestToSubmitHaSCToService"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ItkDispatchRequest, SubmitHaSCToService>()
                .ForMember(dest => dest.SubmitEncounterToServiceRequest, opt => opt.MapFrom(src => src));
         
            Mapper.CreateMap<Authentication, ItkDispatcherSOAPService.Authentication>();

        }
    }
}