using System;
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
            var user = new ApplicationUser(Level.Advanced, DanceGender.Male)
            {
                Id = "mockId"
            };
            handle.MockSet.Setup(m => m.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<ApplicationUser>(user));

            var time = DateTime.Now;

            var command = new SetUserAttendingCommand("mockId")
            {
                Attending = time
            };

            var commandHandler = new SetUserAttendingCommandHandler(handle.MockContext.Object);

            //Act
            var x = await commandHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.AreEqual(time, user.Attending);
        }

        [Test]
        public async Task SetExistingUserAttendingWithNull()
        {
            //Arrange


            using var handle = new UnitTestHandle();
            var user = new ApplicationUser(Level.Advanced, DanceGender.Male)
            {
                Id = "mockId"
            };
            handle.MockSet.Setup(m => m.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<ApplicationUser>(user));

            var command = new SetUserAttendingCommand("mockId");

            var commandHandler = new SetUserAttendingCommandHandler(handle.MockContext.Object);

            //Act
            var x = await commandHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.AreEqual(null, user.Attending);
            handle.MockContext.Verify((n) => n.SaveChangesAsync(CancellationToken.None), Times.Once());
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