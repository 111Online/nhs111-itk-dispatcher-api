using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Castle.Components.DictionaryAdapter;
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

        private ItkDispatchRequest _basicRequest;


        [SetUp]
        public void InitilaiseConverters()
        {
            _basicRequest = new ItkDispatchRequest()
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
                },
                CaseDetails = new CaseDetails()
                {
                    DispositionCode = "Dx123",
                    DispositionName = "Test Disposition"
                }
            };
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
        public void Map_ITKDispatchRequest_To_ToSubmitHaSCToService_with_ReportText()
        {
            var requestWithReportItems = _basicRequest;
            requestWithReportItems.CaseDetails.ReportItems = new List<string>() {"Report line 1", "Report line 2"};

            var result = _mapper.Map<ItkDispatchRequest, SubmitHaSCToService>(requestWithReportItems);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Authentication);
            Assert.IsNotNull(result.SubmitEncounterToServiceRequest);
            Assert.IsNotNull(result.SubmitEncounterToServiceRequest.CaseDetails);
            Assert.AreEqual(2,result.SubmitEncounterToServiceRequest.CaseDetails.CaseSummary.Count());

            Assert.AreEqual("Report line 1", result.SubmitEncounterToServiceRequest.CaseDetails.CaseSummary.Where(c => c.Name == "ReportText").First().Caption);
            Assert.IsTrue(result.SubmitEncounterToServiceRequest.CaseDetails.CaseSummary.Any(c => c.Name == "ReportText"), "CaseDetails does not contain summary item with a name of 'ReportText'");
            Assert.AreEqual("Report line 1", result.SubmitEncounterToServiceRequest.CaseDetails.CaseSummary.Where(c => c.Name == "ReportText").First().Values[0]);

        }

        [Test]
        public void Map_ITKDispatchRequest_To_ToSubmitHaSCToService_with_ConsultationItems()
        {
            var requestWithReportItems = _basicRequest;
            requestWithReportItems.CaseDetails.ConsultationSummaryItems = new List<string>() { "Consult line 1", "Consult line 2" }; ;

            var result = _mapper.Map<ItkDispatchRequest, SubmitHaSCToService>(requestWithReportItems);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Authentication);
            Assert.IsNotNull(result.SubmitEncounterToServiceRequest);
            Assert.IsNotNull(result.SubmitEncounterToServiceRequest.CaseDetails);
            Assert.AreEqual(2, result.SubmitEncounterToServiceRequest.CaseDetails.CaseSummary.Count());

            Assert.AreEqual("Consult line 1", result.SubmitEncounterToServiceRequest.CaseDetails.CaseSummary.Where(c => c.Name == "DispositionDisplayText").First().Caption);
            Assert.IsTrue(result.SubmitEncounterToServiceRequest.CaseDetails.CaseSummary.Any(c => c.Name == "DispositionDisplayText"), "CaseDetails does not contain summary item with a name of 'DispositionDisplayText'");
            Assert.AreEqual("Consult line 1", result.SubmitEncounterToServiceRequest.CaseDetails.CaseSummary.Where(c => c.Name == "DispositionDisplayText").First().Values[0]);


        }

        [Test]
        public void Map_ITKDispatchRequest_To_ToSubmitHaSCToService_With_Null_ReportItems()
        {
            var result = _mapper.Map<ItkDispatchRequest, SubmitHaSCToService>(_basicRequest);
            Assert.IsNotNull(result);
            Assert.IsNull(result.SubmitEncounterToServiceRequest.CaseDetails.CaseSummary);
        }

    }
}
