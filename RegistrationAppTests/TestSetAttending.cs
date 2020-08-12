using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using RegistrationApp.Messaging.Commands.SetUserAttending;
using RegistrationAppDAL.Data;
using RegistrationAppDAL.Models;

namespace RegistrationAppTests
{
    public class Tests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task SetExistingUserAttendingWithDateTime()
        {
            //Arrange

            using var handle = new UnitTestHandle();
            var user = new ApplicationUser(DanceGender.Male)
            {
                Levels = new List<string> { Level.Advanced },
                Id = "mockId"
            };
            handle.MockSet.Setup(m => m.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<ApplicationUser>(user));

            var time = DateTime.Now;

            var command = new SetUserAttendingCommand("mockId"){
                Attending = new Attending(time)
                {
                    Levels = new List<string> { Level.Advanced }
                }
            };

            var commandHandler = new SetUserAttendingCommandHandler(handle.MockContext.Object);

            //Act
            var x = await commandHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.AreNotEqual(null, user.Attending);
            Assert.AreEqual(time, user.Attending!.Date);
        }

        [Test]
        public void SetNullAttending()
        {
            //Arrange
            using var handle = new UnitTestHandle();

            var command = new SetUserAttendingCommand("mockId");

            var commandHandler = new SetUserAttendingCommandHandler(handle.MockContext.Object);

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await commandHandler.Handle(command, CancellationToken.None));

        }
    }
}