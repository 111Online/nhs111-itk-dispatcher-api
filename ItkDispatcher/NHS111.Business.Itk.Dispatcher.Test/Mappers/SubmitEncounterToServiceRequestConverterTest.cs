using System;
using AutoMapper;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using NHS111.Business.Itk.Dispatcher.Api.Mappings;
using NHS111.Domain.Itk.Dispatcher.Models;
using NUnit.Framework;
using Address = NHS111.Domain.Itk.Dispatcher.Models.Address;
using Authentication = NHS111.Domain.Itk.Dispatcher.Models.Authentication;
using PatientDetails = NHS111.Domain.Itk.Dispatcher.Models.PatientDetails;

namespace NHS111.Business.Itk.Dispatcher.Test.Mappers
{
    public class SubmitEncounterToServiceRequestConverterTest
    {
        private MapperConfiguration _config;
        private IMapper _mapper;

        public const string TEST_PHONE_NUMBER = "02380666666";
        public const string TEST_PATIENT_FORENAME = "PatientForename";
        public const string TEST_PATIENT_SURNAME = "PatientSurname";

        public const string TEST_PATIENT_CURRENT_POSTCODE = "HS1 6HH";
        public const string TEST_PATIENT_CURRENT_STREETADDRESS = "1 home lane";

        public const string TEST_GP_POSTCODE = "TS1 6TH";
        public const string TEST_GP_STREETADDRESS = "1 test gp lane";

        public const string TEST_GP_TELEPHONE = "02380555555";
        public const string TEST_GP_ODS_CODE ="RA286";

        public const int DOS_SERVICE_ID = 1234;
        public const string TEST_SERVICE_ODS_CODE = "Y028240002";

        public const string TEST_EXTERNAL_REF = "REF_123456";
        public const string TEST_DX_CODE = "DX0123";
        public const string TEST_DX_NAME = "Rashes";
   

        [SetUp]
        public void InitilaiseConverters()
        {
            //AppDomain.CurrentDomain.GetAssemblies();
            _config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FromItkDispatchRequestToSubmitEncounterToServiceRequest());
            });
            _mapper = _config.CreateMapper();
        }

        [Test]
        public void FromITKDispatchRequestToSubmitHaSCToService_ConfiguredCorrectly()
        {
            _config.AssertConfigurationIsValid("FromItkDispatchRequestToSubmitEncounterToServiceRequest");
        }

        [Test]
        public void Map_ITKDispatchRequest_To_SubmitEncounterToServiceRequest()
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
                    DateOfBirth = new DateTime(1980, 11, 30),
                    Forename = TEST_PATIENT_FORENAME,
                    Surname = TEST_PATIENT_SURNAME,
                    Gender = "Female",
                    GpPractice = new GpPractice()
                    {
                        Address = new Address()
                        {
                            StreetAddressLine1 = TEST_GP_STREETADDRESS,
                            PostalCode = TEST_GP_POSTCODE
                        },
                        Name = "Test GP Practice",
                        Ods = TEST_GP_ODS_CODE,
                        Telephone = TEST_GP_TELEPHONE
                    },
                    CurrentAddress = new Address()
                    {
                        StreetAddressLine1 = TEST_PATIENT_CURRENT_STREETADDRESS,
                        PostalCode = TEST_PATIENT_CURRENT_POSTCODE
                    },
                    ServiceAddressPostcode = "SV10 6YY",
                    TelephoneNumber = TEST_PHONE_NUMBER
                },
                ServiceDetails = new ServiceDetails()
                {
                    Id = DOS_SERVICE_ID.ToString(),
                    Name = "TestSurgery",
                    PostCode = "TT22 5TT",
                    OdsCode = TEST_SERVICE_ODS_CODE
                },
                CaseDetails = new CaseDetails()
                {
                    ExternalReference = TEST_EXTERNAL_REF,
                    DispositionCode = TEST_DX_CODE,
                    DispositionName = TEST_DX_NAME
                }
            };

            var result = _mapper.Map<ItkDispatchRequest, SubmitEncounterToServiceRequest>(dispatchRequest);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PatientDetails);
            Assert.IsNotNull(result.ServiceDetails);

            Assert.AreEqual(result.PatientDetails.TelephoneNumber, TEST_PHONE_NUMBER);
            Assert.AreEqual(result.PatientDetails.Forename, TEST_PATIENT_FORENAME);
            Assert.AreEqual(result.PatientDetails.Surname, TEST_PATIENT_SURNAME);
            Assert.AreEqual(result.PatientDetails.Gender, gender.Female);
            Assert.AreEqual(result.PatientDetails.DateOfBirth.Item, "1980-11-30");

            Assert.AreEqual(result.PatientDetails.CurrentAddress.PostalCode, TEST_PATIENT_CURRENT_POSTCODE);
            Assert.AreEqual(result.PatientDetails.CurrentAddress.StreetAddressLine1, TEST_PATIENT_CURRENT_STREETADDRESS);

            Assert.AreEqual(result.PatientDetails.GpPractice.Address.PostalCode, TEST_GP_POSTCODE);
            Assert.AreEqual(result.PatientDetails.GpPractice.Address.StreetAddressLine1, TEST_GP_STREETADDRESS);

            Assert.AreEqual(result.PatientDetails.GpPractice.Telephone, TEST_GP_TELEPHONE);
            Assert.AreEqual(result.PatientDetails.GpPractice.ODS, TEST_GP_ODS_CODE);

            Assert.AreEqual(result.ServiceDetails.id, DOS_SERVICE_ID);
            Assert.AreEqual(result.ServiceDetails.odsCode, TEST_SERVICE_ODS_CODE);

            Assert.AreEqual(result.CaseDetails.ExternalReference, TEST_EXTERNAL_REF);
            Assert.AreEqual(result.CaseDetails.DispositionCode, TEST_DX_CODE);
            Assert.AreEqual(result.CaseDetails.DispositionName, TEST_DX_NAME);
        }


    }
}
