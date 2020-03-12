using System;
using System.Net;
using System.Net.Http;
using System.Text;
using NUnit.Framework;

namespace NHS111.Business.Itk.Dispatcher.API.Functional.Tests
{
    using System.Configuration;

    [TestFixture]
    public class ItkDispatcherTests
    {
        private string _domainITKApi = "https://nhs111-beta-itkdispatcher.azurewebsites.net/";
        
        private readonly RestfulHelper _restfulHelper = new RestfulHelper();

        private static string Username
        {
            get
            {
                return ConfigurationManager.AppSettings["itk_credential_user"];
            }
        }

        private static string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["itk_credential_password"];
            }
        }

        //Test to show failure when Name field in service details not sent
        [Test]
        public void TestInvalidITKMessage_Fails()
        {
            var endpoint = "SendItkMessage";
            var itkDisptatchRequest = string.Format("{{\"Authentication\":{{\"UserName\":\"{0}\",\"Password\":\"{1}\"}},\"PatientDetails\":{{\"Forename\":\"TestForename\",\"Surname\":\"TestSurname\",\"Gender\":\"Male\",\"DateOfBirth\":\"11/11/1980\",\"ServiceAddressPostcode\":\"TS196TH\",\"TelephoneNumber\":\"02070 033002\",\"HomeAddress\":{{\"PostalCode\":\"TS176TH\",\"StreetAddressLine1\":\"1 Test Lane\"}},\"GpPractice\":{{\"Name\":\"Test GP\",\"Telephone\":\"02380 666454\",\"ODS\":\"RHYAA\",\"Address\":{{\"PostalCode\":\"GP3 6FF\",\"StreetAddressLine1\":\"1 GP Street\"}}}},\"ServiceDetails\":{{\"Id\":\"158960\",\"OdsCode\":\"13524169111352416911\"}}}}", Username, Password);
            var address = string.Format(_domainITKApi + endpoint);

            ServicePointManager.Expect100Continue = false;
            var result = _restfulHelper.PostAsync(address, CreateHTTPRequest(itkDisptatchRequest));

            Assert.IsNotNull(result.Result);
            Assert.IsFalse(result.Result.IsSuccessStatusCode);
            Assert.AreEqual(result.Result.StatusCode, HttpStatusCode.InternalServerError);
            //these check the wrong fields are not returned

        }
        
        //Test to show failure when invalid authentication is sent
        [Test]
        public void TestUnauthenticatedITKMessage_Fails()
        {
            var endpoint = "SendItkMessage";
            var itkDisptatchRequest = ("{\"Authentication\":{\"UserName\":\"Test\",\"Password\":\"Test\"},\"PatientDetails\":{\"Forename\":\"TestForename\",\"Surname\":\"TestSurname\",\"Gender\":\"Male\",\"DateOfBirth\":\"11/11/1980\",\"ServiceAddressPostcode\":\"TS196TH\",\"TelephoneNumber\":\"02070 033002\",\"HomeAddress\":{\"PostalCode\":\"TS176TH\",\"StreetAddressLine1\":\"1 Test Lane\"},\"GpPractice\":{\"Name\":\"Test GP\",\"Telephone\":\"02380 666454\",\"ODS\":\"RHYAA\",\"Address\":{\"PostalCode\":\"GP3 6FF\",\"StreetAddressLine1\":\"1 GP Street\"}},\"ServiceDetails\":{\"Id\":\"158960\",\"OdsCode\":\"13524169111352416911\"}}");
            var address = String.Format(_domainITKApi + endpoint);

            ServicePointManager.Expect100Continue = false;
            var result = _restfulHelper.PostAsync(address, CreateHTTPRequest(itkDisptatchRequest));

            Assert.IsNotNull(result.Result);
            Assert.IsFalse(result.Result.IsSuccessStatusCode);
            Assert.AreEqual(result.Result.StatusCode, HttpStatusCode.InternalServerError);

        }
        
        public static HttpRequestMessage CreateHTTPRequest(string requestContent)
        {
            return new HttpRequestMessage
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };
        }
    }
}
