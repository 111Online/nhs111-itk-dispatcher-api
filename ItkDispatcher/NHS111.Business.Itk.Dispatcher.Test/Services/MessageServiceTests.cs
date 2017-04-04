﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NHS111.Domain.Itk.Dispatcher.Models;
using NHS111.Domain.Itk.Dispatcher.Services;
using NUnit.Framework;

namespace NHS111.Business.Itk.Dispatcher.Test.Services
{
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
            Journey journey = null;
            _moqAzureStorageService.Setup(s => s.GetHash(It.IsAny<string>())).Returns(Task.FromResult(journey));

            var srv = new MessageService(_moqAzureStorageService.Object);

            var result = srv.MessageAlreadyExists(string.Empty, string.Empty);
            Assert.IsFalse(result.Result);
        }

        [Test]
        public void Existing_message_matches_returns_true()
        {
            var journey = new Journey() { Id = "123456789", Hash = "82DFA5549EBC9AFC168EB7931EBECE" };
            _moqAzureStorageService.Setup(s => s.GetHash(It.IsAny<string>())).Returns(Task.FromResult(journey));

            var srv = new MessageService(_moqAzureStorageService.Object);

            var result = srv.MessageAlreadyExists("123456789", "Test message");
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void Existing_message_doesnt_match_return_false()
        {
            var journey = new Journey() { Id = "123456789", Hash = "82DFA5549EBC9AFC168EB7931EBECE" };
            _moqAzureStorageService.Setup(s => s.GetHash(It.IsAny<string>())).Returns(Task.FromResult(journey));

            var srv = new MessageService(_moqAzureStorageService.Object);

            var result = srv.MessageAlreadyExists("123456789", "Test message two");
            Assert.IsFalse(result.Result);
        }
    }
}
