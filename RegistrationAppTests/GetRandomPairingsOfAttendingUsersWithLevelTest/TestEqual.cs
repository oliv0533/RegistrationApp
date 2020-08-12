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
                new ApplicationUser(new List<Level>{Level.Beginner}, DanceGender.Male)
                {
                    Attending = new Attending(new List<Level>{Level.Beginner}, DateTime.Now)
                },
                new ApplicationUser(new List<Level>{Level.Beginner}, DanceGender.Female)
                {
                    Attending = new Attending(new List<Level>{Level.Beginner}, DateTime.Now)
                },
                new ApplicationUser(new List<Level>{Level.Beginner}, DanceGender.Male)
                {
                    Attending = new Attending(new List<Level>{Level.Beginner}, DateTime.Now)
                },
                new ApplicationUser(new List<Level>{Level.Beginner}, DanceGender.Female)
                {
                    Attending = new Attending(new List<Level>{Level.Beginner}, DateTime.Now)
                },
            }.AsQueryable();

            TestHelper.SetupData(data, testHandle);

            testHandle.MockContext.Setup(c => c.Users).Returns(testHandle.MockSet.Object);

            var command = new GetRandomPairingsOfAttendingUsersWithLevelQuery(new List<Level>{Level.Beginner});

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

            var firstBoy = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Male)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now),
                Id = "first-boy"
            };
            var firstGirl = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Female)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now),
                Id = "first-girl"
            };
            var secondBoy = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Male)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now)
            };
            var secondGirl = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Female)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now)
            };

            
            data.Add(firstBoy);
            data.Add(firstGirl);
            data.Add(secondBoy);
            data.Add(secondGirl);

            firstBoy.FormerMatches.Add(new FormerMatch(firstGirl.Id, time));
            firstGirl.FormerMatches.Add(new FormerMatch(firstBoy.Id, time));

            TestHelper.SetupData(data.AsQueryable(), testHandle);

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

            var firstBoy = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Male)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now),
                Id = "first-boy"
            };
            var firstGirl = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Female)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now),
                Id = "first-girl"
            };
            var secondBoy = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Male)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now)
            };
            var secondGirl = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Female)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, DateTime.Now)
            };

            data.Add(secondBoy);
            data.Add(secondGirl);
            data.Add(firstBoy);
            data.Add(firstGirl);
            

            firstBoy.FormerMatches.Add(new FormerMatch(firstGirl.Id, time));
            firstGirl.FormerMatches.Add(new FormerMatch(firstBoy.Id, time));

            TestHelper.SetupData(data.AsQueryable(), testHandle);

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
            Assert.AreEqual(4, result.Count);
            Assert.AreNotEqual(null, result.First(x => x.Id == firstBoy.Id).Match);
            Assert.AreNotEqual(firstGirl.Id, result.First(x => x.Id == firstBoy.Id).Match!.Id);
        }

        [Test]
        public async Task TestEqualBoysAndGirlsWithManyFormerPartners()
        {
            //Arrange
            using var testHandle = new UnitTestHandle();

            var data = new List<ApplicationUser>();

            var time = DateTime.Now;

            var firstBoy = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Male)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, time),
                Id = "first-boy"
            };
            var firstGirl = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Female)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, time),
                Id = "first-girl"
            };
            var secondBoy = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Male)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, time),
                Id = "second-boy"
            };
            var secondGirl = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Female)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, time),
                Id = "second-girl"
            };
            var thirdBoy = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Male)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, time),
                Id = "third-boy"
            };
            var thirdGirl = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Female)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, time),
                Id = "third-girl"
            };
            var fourthBoy = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Male)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, time),
                Id = "fourth-boy"
            };
            var fourthGirl = new ApplicationUser(new List<Level> { Level.Beginner }, DanceGender.Female)
            {
                Attending = new Attending(new List<Level> { Level.Beginner }, time),
                Id = "fourth-girl"
            };



            data.Add(secondBoy);
            data.Add(secondGirl);
            data.Add(firstBoy);
            data.Add(firstGirl);
            data.Add(thirdBoy);
            data.Add(thirdGirl);
            data.Add(fourthBoy);
            data.Add(fourthGirl);

            firstBoy.FormerMatches.AddRange(new List<FormerMatch>
            {
                new FormerMatch(firstGirl.Id, time), 
                new FormerMatch(secondGirl.Id, time),
                new FormerMatch(secondGirl.Id, time),
                new FormerMatch(thirdGirl.Id, time)
            });

            firstGirl.FormerMatches.AddRange(new List<FormerMatch>
            {
                new FormerMatch(firstBoy.Id, time),
                new FormerMatch(thirdBoy.Id, time),
                new FormerMatch(fourthBoy.Id, time)
            });

            secondBoy.FormerMatches.AddRange(new List<FormerMatch>
            {
                new FormerMatch(secondGirl.Id, time),
                new FormerMatch(secondGirl.Id, time),
                new FormerMatch(fourthGirl.Id, time)
            });

            secondGirl.FormerMatches.AddRange(new List<FormerMatch>
            {
                new FormerMatch(secondBoy.Id, time),
                new FormerMatch(secondBoy.Id, time),
                new FormerMatch(firstBoy.Id, time),
                new FormerMatch(firstBoy.Id, time)
            });

            thirdBoy.FormerMatches.AddRange(new List<FormerMatch>
            {
                new FormerMatch(firstGirl.Id, time),
                new FormerMatch(fourthGirl.Id, time),
            });

            thirdGirl.FormerMatches.AddRange(new List<FormerMatch>
            {
                new FormerMatch(firstBoy.Id, time),
                new FormerMatch(fourthBoy.Id, time),
                new FormerMatch(fourthBoy.Id, time)
            });

            fourthBoy.FormerMatches.AddRange(new List<FormerMatch>
            {
                new FormerMatch(firstGirl.Id, time),
                new FormerMatch(thirdGirl.Id, time),
                new FormerMatch(thirdGirl.Id, time)
            });

            fourthGirl.FormerMatches.AddRange(new List<FormerMatch>
            {
                new FormerMatch(secondBoy.Id, time),
                new FormerMatch(thirdBoy.Id, time)
            });

            TestHelper.SetupData(data.AsQueryable(), testHandle);

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
            Assert.AreEqual(8, result.Count);
            Assert.AreEqual(fourthGirl.Id, result.First(x => x.Id == firstBoy.Id).Match!.Id);
            Assert.AreEqual(firstGirl.Id, result.First(x => x.Id == secondBoy.Id).Match!.Id);
            Assert.AreEqual(thirdGirl.Id, result.First(x => x.Id == thirdBoy.Id).Match!.Id);
            Assert.AreEqual(secondGirl.Id, result.First(x => x.Id == fourthBoy.Id).Match!.Id);
        }
    }
}