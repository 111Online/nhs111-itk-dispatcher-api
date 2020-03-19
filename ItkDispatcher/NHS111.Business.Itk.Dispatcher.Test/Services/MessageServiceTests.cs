using Moq;

using NHS111.Domain.Itk.Dispatcher.Services;
using NUnit.Framework;

namespace NHS111.Business.Itk.Dispatcher.Test.Services
{
    using Microsoft.WindowsAzure.Storage.Table;

    [TestFixture]
    public class MessageServiceTests
    {
        private Mock<IAzureStorageService> _moqAzureStorageService;

        [SetUp]
        public void SetUp()
        {
            _moqAzureStorageService = new Mock<IAzureStorageService>();
        }

        [Test]
        public void No_existing_message_returns_false()
        {
            _moqAzureStorageService.Setup(s => s.EntityExists(It.IsAny<TableEntity>())).Returns(false);

            var srv = new MessageService(_moqAzureStorageService.Object);

            var result = srv.MessageAlreadyExists(string.Empty);

            Assert.IsFalse(result);
        }

        [Test]
        public void Existing_message_matches_returns_true()
        {
            _moqAzureStorageService.Setup(s => s.EntityExists(It.IsAny<TableEntity>())).Returns(true);

            var srv = new MessageService(_moqAzureStorageService.Object);

            var result = srv.MessageAlreadyExists("Test message");

            Assert.IsTrue(result);
        }

        [Test]
        public void Existing_message_doesnt_match_return_false()
        {
            _moqAzureStorageService.Setup(s => s.EntityExists(It.IsAny<TableEntity>())).Returns(false);

            var srv = new MessageService(_moqAzureStorageService.Object);

            var result = srv.MessageAlreadyExists("Test message two");

            Assert.IsFalse(result);
        }
    }
}
