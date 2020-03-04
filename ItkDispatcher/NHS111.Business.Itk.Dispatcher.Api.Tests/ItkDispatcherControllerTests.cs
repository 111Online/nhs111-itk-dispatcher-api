using System;
using Xunit;

namespace NHS111.Business.Itk.Dispatcher.Api.Test
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Builders;
    using Controllers;
    using Domain.Itk.Dispatcher.Models;
    using Domain.Itk.Dispatcher.Services;
    using ItkDispatcherSOAPService;
    using log4net;
    using Mappings;
    using Moq;

    public class ItkDispatcherControllerTests
    {
        private readonly Mock<MessageEngine> _mockMessageEngine;
        private readonly Mock<IItkDispatchResponseBuilder> _mockItkDispatchResponseBuilder;
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly Mock<IPatientReferenceService> _mockPatientReferenceService;
        private readonly Mock<ILog> _mockLog;

        public ItkDispatcherControllerTests()
        {
            _mockMessageEngine = new Mock<MessageEngine>();
            _mockItkDispatchResponseBuilder = new Mock<IItkDispatchResponseBuilder>();
            _mockMessageService = new Mock<IMessageService>();
            _mockPatientReferenceService = new Mock<IPatientReferenceService>();
            _mockLog = new Mock<ILog>();

            AutoMapperWebConfiguration.Configure();
        }

        [Fact]
        public async Task SendItkMessage_WhenCovidPathway_CallsStoreRequestAsync() {
            _mockItkDispatchResponseBuilder.Setup(i => i.Build(It.IsAny<SubmitHaSCToServiceResponse>(), It.IsAny<string>()))
                .Returns(new ItkDispatchResponse {StatusCode = HttpStatusCode.OK});

            var sut = new ItkDispatcherController(_mockMessageEngine.Object, _mockItkDispatchResponseBuilder.Object,
                _mockMessageService.Object, _mockPatientReferenceService.Object, _mockLog.Object);

            var model = new ItkDispatchRequest {
                CaseDetails = new CaseDetails {
                    JourneyId = "123",
                    DispositionCode = "DxCV192"
                },
                PatientDetails = new Domain.Itk.Dispatcher.Models.PatientDetails()
            };
            var response = await sut.SendItkMessage(model);

            _mockMessageService.Verify(m => m.MessageAlreadyExists(It.Is<string>(j => j == model.CaseDetails.JourneyId), It.IsAny<string>()), Times.Once);
            _mockMessageEngine.Verify(m => m.SubmitHaSCToServiceAsync(It.IsAny<SubmitHaSCToService>()), Times.Never);
            _mockMessageService.Verify(m => m.StoreRequestAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMessageService.Verify(m => m.StoreMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task SendItkMessage_WhenNotCovidPathway_CallsSubmitHaSCToServiceAsync() {
            _mockItkDispatchResponseBuilder.Setup(i => i.Build(It.IsAny<SubmitHaSCToServiceResponse>(), It.IsAny<string>()))
                .Returns(new ItkDispatchResponse {StatusCode = HttpStatusCode.OK});

            var sut = new ItkDispatcherController(_mockMessageEngine.Object, _mockItkDispatchResponseBuilder.Object,
                _mockMessageService.Object, _mockPatientReferenceService.Object, _mockLog.Object);

            var model = new ItkDispatchRequest {
                CaseDetails = new CaseDetails {
                    JourneyId = "123",
                    DispositionCode = "Dx012"
                },
                PatientDetails = new Domain.Itk.Dispatcher.Models.PatientDetails()
            };
            var response = await sut.SendItkMessage(model);

            _mockMessageService.Verify(m => m.MessageAlreadyExists(It.Is<string>(j => j == model.CaseDetails.JourneyId), It.IsAny<string>()), Times.Once);
            _mockMessageEngine.Verify(m => m.SubmitHaSCToServiceAsync(It.IsAny<SubmitHaSCToService>()), Times.Once);
            _mockMessageService.Verify(m => m.StoreRequestAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockMessageService.Verify(m => m.StoreMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}