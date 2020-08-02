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
    public class TestEqual
    {
        [Test]
        public async Task TestEqualBoysAndGirls()
        {
            //Arrange
            using var testHandle = new UnitTestHandle();

            var data = new List<ApplicationUser>
            {
                new ApplicationUser(Level.Beginner, DanceGender.Male)
                {
                    Attending = DateTime.Now
                },
                new ApplicationUser(Level.Beginner, DanceGender.Female)
                {
                    Attending = DateTime.Now
                },
                new ApplicationUser(Level.Beginner, DanceGender.Male)
                {
                    Attending = DateTime.Now
                },
                new ApplicationUser(Level.Beginner, DanceGender.Female)
                {
                    Attending = DateTime.Now
                },
            }.AsQueryable();

            TestHelper.SetupData(data, testHandle);

            testHandle.MockContext.Setup(c => c.Users).Returns(testHandle.MockSet.Object);

            var command = new GetRandomPairingsOfAttendingUsersWithLevelQuery(Level.Beginner);

            var mediator = new Mock<IMediator>();

            mediator
                .Setup(x => x
                    .Send(It.IsAny<GetAllAttendingUsersWithLevelQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(data.ToList()));

            var commandHandler = new GetRandomPairingsOfAttendingUsersWithLevelQueryHandler(mediator.Object);

            //Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.AreEqual(4, result.Count);
        }

        [Test]
        public async Task TestEqualBoysAndGirlsWithSomeFormerPartners()
        {
            //Arrange
            using var testHandle = new UnitTestHandle();

            var data = new List<ApplicationUser>();

            var time = DateTime.Now;

            var firstBoy = new ApplicationUser(Level.Beginner, DanceGender.Male)
            {
                Attending = time,
                Id = "first-boy"
            };
            var firstGirl = new ApplicationUser(Level.Beginner, DanceGender.Female)
            {
                Attending = time,
                Id = "first-girl"
            };
            var secondBoy = new ApplicationUser(Level.Beginner, DanceGender.Male)
            {
                Attending = time
            };
            var secondGirl = new ApplicationUser(Level.Beginner, DanceGender.Female)
            {
                Attending = time
            };

            
            data.Add(firstBoy);
            data.Add(firstGirl);
            data.Add(secondBoy);
            data.Add(secondGirl);

            firstBoy.FormerMatches.Add(new FormerMatch(firstGirl.Id, time));
            firstGirl.FormerMatches.Add(new FormerMatch(firstBoy.Id, time));

            TestHelper.SetupData(data.AsQueryable(), testHandle);

            testHandle.MockContext.Setup(c => c.Users).Returns(testHandle.MockSet.Object);

            var command = new GetRandomPairingsOfAttendingUsersWithLevelQuery(Level.Beginner);

            var mediator = new Mock<IMediator>();

            mediator
                .Setup(x => x
                    .Send(It.IsAny<GetAllAttendingUsersWithLevelQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(data.ToList()));

            var commandHandler = new GetRandomPairingsOfAttendingUsersWithLevelQueryHandler(mediator.Object);

            //Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.AreEqual(4, result.Count);
            Assert.AreNotEqual(null, result.First(x => x.Id == firstBoy.Id).Match);
            Assert.AreNotEqual(firstGirl.Id, result.First(x => x.Id == firstBoy.Id).Match!.Id);
        }

        [Test]
        public async Task TestEqualBoysAndGirlsWithSomeFormerPartnersPartnerLessFirst()
        {
            //Arrange
            using var testHandle = new UnitTestHandle();

            var data = new List<ApplicationUser>();

            var time = DateTime.Now;

            var firstBoy = new ApplicationUser(Level.Beginner, DanceGender.Male)
            {
                Attending = time,
                Id = "first-boy"
            };
            var firstGirl = new ApplicationUser(Level.Beginner, DanceGender.Female)
            {
                Attending = time,
                Id = "first-girl"
            };
            var secondBoy = new ApplicationUser(Level.Beginner, DanceGender.Male)
            {
                Attending = time
            };
            var secondGirl = new ApplicationUser(Level.Beginner, DanceGender.Female)
            {
                Attending = time
            };

            data.Add(secondBoy);
            data.Add(secondGirl);
            data.Add(firstBoy);
            data.Add(firstGirl);
            

            firstBoy.FormerMatches.Add(new FormerMatch(firstGirl.Id, time));
            firstGirl.FormerMatches.Add(new FormerMatch(firstBoy.Id, time));

            TestHelper.SetupData(data.AsQueryable(), testHandle);

            testHandle.MockContext.Setup(c => c.Users).Returns(testHandle.MockSet.Object);

            var command = new GetRandomPairingsOfAttendingUsersWithLevelQuery(Level.Beginner);

            var mediator = new Mock<IMediator>();

            mediator
                .Setup(x => x
                    .Send(It.IsAny<GetAllAttendingUsersWithLevelQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(data.ToList()));

            var commandHandler = new GetRandomPairingsOfAttendingUsersWithLevelQueryHandler(mediator.Object);

            //Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.AreEqual(4, result.Count);
            Assert.AreNotEqual(null, result.First(x => x.Id == firstBoy.Id).Match);
            Assert.AreNotEqual(firstGirl.Id, result.First(x => x.Id == firstBoy.Id).Match!.Id);
        }
    }
}