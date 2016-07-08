using System;
using AutoMapper;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using NHS111.Business.Itk.Dispatcher.Api.Mappings;
using NHS111.Domain.Itk.Dispatcher.Models;
using NUnit.Framework;
using Address = NHS111.Domain.Itk.Dispatcher.Models.Address;
using Authentication = NHS111.Domain.Itk.Dispatcher.Models.Authentication;
using FromItkDispatchRequestToSubmitEncounterToServiceRequest = NHS111.Business.Itk.Dispatcher.Api.Mappings.FromItkDispatchRequestToSubmitEncounterToServiceRequest;
using PatientDetails = NHS111.Domain.Itk.Dispatcher.Models.PatientDetails;

namespace NHS111.Business.Itk.Dispatcher.Test.Mappers
{
    [TestFixture]
    public class ItkDispatcherMapperTest
    {
        private MapperConfiguration _config;
        private IMapper _mapper;
        
        [SetUp]
        public void InitilaiseConverters()
        {
            //AppDomain.CurrentDomain.GetAssemblies();
            _config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FromItkDispatchRequestToSubmitHaScToService());
                cfg.AddProfile(new FromItkDispatchRequestToSubmitEncounterToServiceRequest());
            });
            _mapper = _config.CreateMapper();
        }

        [Test]
        public void FromITKDispatchRequestToSubmitHaSCToService_ConfiguredCorrectly()
        {
            _config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FromItkDispatchRequestToSubmitHaScToService());
                cfg.AddProfile(new FromItkDispatchRequestToSubmitEncounterToServiceRequest());
            });
            _mapper = _config.CreateMapper();
            _config.AssertConfigurationIsValid("FromITKDispatchRequestToSubmitHaSCToService");
        }

        [Test]
        public void Map_ITKDispatchRequest_To_ToSubmitHaSCToService()
        {
            var dispatchRequest = new ItkDispatchRequest()
            {
                Authentication = new Authentication()
                {
                    Password = "testPass",
                    UserName = "testUser"
                },
                PatientDetails = new PatientDetails()
                {
                    DateOfBirth = new DateTime(1980, 1, 1),
                    Forename = "PatientForename",
                    Surname = "PatientSurname",
                    Gender = "Male",
                    GpPractice = new GpPractice()
                    {
                        Address = new Address()
                        {
                            StreetAddressLine1 = "1 test lane",
                            PostalCode = "TS1 6TH"
                        },
                        Name = "Test GP Practice",
                        Ods = "RA286",
                        Telephone = "02380555555"
                    },
                    HomeAddress = new Address()
                    {
                        StreetAddressLine1 = "1 home lane",
                        PostalCode = "HS1 6HH"
                    },
                    ServiceAddressPostcode = "SV10 6YY",
                    TelephoneNumber = "02380666666"
                },
                ServiceDetails = new ServiceDetails()
                {
                    Id = "1234",
                    Name = "TestSurgery",
                    PostCode = "TT22 5TT"
                }
            };

            var result = _mapper.Map<ItkDispatchRequest, SubmitHaSCToService>(dispatchRequest);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Authentication);
            Assert.IsNotNull(result.SubmitEncounterToServiceRequest);
        }

    }
}
