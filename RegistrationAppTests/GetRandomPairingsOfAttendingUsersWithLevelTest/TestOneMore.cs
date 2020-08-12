using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using RegistrationApp.Messaging.Queries.GetAllAttendingUsersWithLevel;
using RegistrationApp.Messaging.Queries.GetRandomPairingsOfAttendingUsersWithLevel;
using RegistrationAppDAL.Models;

namespace RegistrationAppTests.GetRandomPairingsOfAttendingUsersWithLevelTest
{
    public class TestOneMore
    {
        [Test]
        public async Task TestOneMoreGirlThanBoy()
        {
            //Arrange
            using var testHandle = new UnitTestHandle();

            var data = new List<ApplicationUser>
            {
                new ApplicationUser(new List<Level>{Level.Beginner}, DanceGender.Male)
                {
                    Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now)
                },
                new ApplicationUser(new List<Level>{Level.Beginner}, DanceGender.Female)
                {
                    Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now)
                },
                new ApplicationUser(new List<Level>{Level.Beginner}, DanceGender.Female)
                {
                    Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now),
                    Id = "some-id"
                },
            }.AsQueryable();

            TestHelper.SetupData(data, testHandle);

            testHandle.MockContext.Setup(c => c.Users).Returns(testHandle.MockSet.Object);

            var command = new GetRandomPairingsOfAttendingUsersWithLevelQuery(new List<Level> { Level.Beginner });

            var mediator = new Mock<IMediator>();

            mediator
                .Setup(x => x
                    .Send(It.IsAny<GetAllAttendingUsersWithLevelQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(data.ToList()));

            var commandHandler = new GetRandomPairingsOfAttendingUsersWithLevelQueryHandler(mediator.Object);

            //Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(result.FirstOrDefault(x => x.Id == "some-id"), null);
        }

        [Test]
        public async Task TestOneMoreBoyThanGirl()
        {
            //Arrange
            using var testHandle = new UnitTestHandle();

            var data = new List<ApplicationUser>
            {
                new ApplicationUser(new List<Level>{Level.Beginner}, DanceGender.Male)
                {
                    Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now)
                },
                new ApplicationUser(new List<Level>{Level.Beginner}, DanceGender.Male)
                {
                    Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now),
                    Id = "some-id"
                },
                new ApplicationUser(new List<Level>{Level.Beginner}, DanceGender.Female)
                {
                    Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now)
                },
            }.AsQueryable();

            TestHelper.SetupData(data, testHandle);

            testHandle.MockContext.Setup(c => c.Users).Returns(testHandle.MockSet.Object);

            var command = new GetRandomPairingsOfAttendingUsersWithLevelQuery(new List<Level> { Level.Beginner });

            var mediator = new Mock<IMediator>();

            mediator
                .Setup(x => x
                    .Send(It.IsAny<GetAllAttendingUsersWithLevelQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(data.ToList()));

            var commandHandler = new GetRandomPairingsOfAttendingUsersWithLevelQueryHandler(mediator.Object);

            //Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(result.FirstOrDefault(x => x.Id == "some-id"), null);
        }
    }
}