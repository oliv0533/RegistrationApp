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
                new ApplicationUser(DanceGender.Male)
                {
                    Levels = new List<string>{Level.Beginner},
                    Attending = new Attending(DateTime.Now)
                    {
                        Levels = new List<string>{Level.Beginner}
                    }
                },
                new ApplicationUser(DanceGender.Female)
                {
                    Levels = new List<string>{Level.Beginner},
                    Attending = new Attending(DateTime.Now)
                    {
                        Levels = new List<string>{Level.Beginner}
                    }
                },
                new ApplicationUser(DanceGender.Male)
                {
                    Levels = new List<string>{Level.Beginner},
                    Attending = new Attending(DateTime.Now)
                    {
                        Levels = new List<string>{Level.Beginner}
                    }
                },
                new ApplicationUser(DanceGender.Female)
                {
                    Levels = new List<string>{Level.Beginner},
                    Attending = new Attending(DateTime.Now)
                    {
                        Levels = new List<string>{Level.Beginner}
                    }
                },
            }.AsQueryable();

            TestHelper.SetupData(data, testHandle);

            testHandle.MockContext.Setup(c => c.Users).Returns(testHandle.MockSet.Object);

            var command = new GetRandomPairingsOfAttendingUsersWithLevelQuery(new List<string>{Level.Beginner});

            var mediator = new Mock<IMediator>();

            mediator
                .Setup(x => x
                    .Send(It.IsAny<GetAllAttendingUsersWithLevelQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(data.ToList()));

            var commandHandler = new GetRandomPairingsOfAttendingUsersWithLevelQueryHandler(mediator.Object);

            //Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.AreEqual(2, result.Pairings.Count);
        }

        [Test]
        public async Task TestEqualBoysAndGirlsWithSomeFormerPartners()
        {
            //Arrange
            using var testHandle = new UnitTestHandle();

            var data = new List<ApplicationUser>();

            var time = DateTime.Now;

            var firstBoy = new ApplicationUser(DanceGender.Male)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(DateTime.Now)
                {
                    Levels = new List<string> { Level.Beginner }
                },
                Id = "first-boy"
            };
            var firstGirl = new ApplicationUser(DanceGender.Female)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(DateTime.Now)
                {
                    Levels = new List<string> { Level.Beginner }
                },
                Id = "first-girl"
            };
            var secondBoy = new ApplicationUser(DanceGender.Male)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(DateTime.Now)
                {
                    Levels = new List<string> { Level.Beginner }
                }
            };
            var secondGirl = new ApplicationUser(DanceGender.Female)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(DateTime.Now)
                {
                    Levels = new List<string> { Level.Beginner }
                }
            };

            
            data.Add(firstBoy);
            data.Add(firstGirl);
            data.Add(secondBoy);
            data.Add(secondGirl);

            firstBoy.FormerMatches.Add(new FormerMatch(firstGirl.Id, time));
            firstGirl.FormerMatches.Add(new FormerMatch(firstBoy.Id, time));

            TestHelper.SetupData(data.AsQueryable(), testHandle);

            testHandle.MockContext.Setup(c => c.Users).Returns(testHandle.MockSet.Object);

            var command = new GetRandomPairingsOfAttendingUsersWithLevelQuery(new List<string> { Level.Beginner });

            var mediator = new Mock<IMediator>();

            mediator
                .Setup(x => x
                    .Send(It.IsAny<GetAllAttendingUsersWithLevelQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(data.ToList()));

            var commandHandler = new GetRandomPairingsOfAttendingUsersWithLevelQueryHandler(mediator.Object);

            //Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.AreEqual(2, result.Pairings.Count);
            Assert.AreNotEqual(firstGirl.Id, result.Pairings.First(x => x.Male.Id == firstBoy.Id).Female.Id);
        }

        [Test]
        public async Task TestEqualBoysAndGirlsWithSomeFormerPartnersPartnerLessFirst()
        {
            //Arrange
            using var testHandle = new UnitTestHandle();

            var data = new List<ApplicationUser>();

            var time = DateTime.Now;

            var firstBoy = new ApplicationUser(DanceGender.Male)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(DateTime.Now)
                {
                    Levels = new List<string> { Level.Beginner }
                },
                Id = "first-boy"
            };
            var firstGirl = new ApplicationUser(DanceGender.Female)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(DateTime.Now)
                {
                    Levels = new List<string> { Level.Beginner }
                },
                Id = "first-girl"
            };
            var secondBoy = new ApplicationUser(DanceGender.Male)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(DateTime.Now)
                {
                    Levels = new List<string> { Level.Beginner }
                },
            };
            var secondGirl = new ApplicationUser(DanceGender.Female)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(DateTime.Now)
                {
                    Levels = new List<string> { Level.Beginner }
                },
            };

            data.Add(secondBoy);
            data.Add(secondGirl);
            data.Add(firstBoy);
            data.Add(firstGirl);
            

            firstBoy.FormerMatches.Add(new FormerMatch(firstGirl.Id, time));
            firstGirl.FormerMatches.Add(new FormerMatch(firstBoy.Id, time));

            TestHelper.SetupData(data.AsQueryable(), testHandle);

            testHandle.MockContext.Setup(c => c.Users).Returns(testHandle.MockSet.Object);

            var command = new GetRandomPairingsOfAttendingUsersWithLevelQuery(new List<string> { Level.Beginner });

            var mediator = new Mock<IMediator>();

            mediator
                .Setup(x => x
                    .Send(It.IsAny<GetAllAttendingUsersWithLevelQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(data.ToList()));

            var commandHandler = new GetRandomPairingsOfAttendingUsersWithLevelQueryHandler(mediator.Object);

            //Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.AreEqual(2, result.Pairings.Count);
            Assert.NotNull(result.Pairings.First(x => x.Male.Id == firstBoy.Id).Female);
            Assert.AreNotEqual(firstGirl.Id, result.Pairings.First(x => x.Male.Id == firstBoy.Id).Female.Id);
        }

        [Test]
        public async Task TestEqualBoysAndGirlsWithManyFormerPartners()
        {
            //Arrange
            using var testHandle = new UnitTestHandle();

            var data = new List<ApplicationUser>();

            var time = DateTime.Now;

            var firstBoy = new ApplicationUser(DanceGender.Male)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(time)
                {
                    Levels = new List<string> { Level.Beginner }
                },
                Id = "first-boy"
            };
            var firstGirl = new ApplicationUser(DanceGender.Female)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(time)
                {
                    Levels = new List<string> { Level.Beginner }
                },
                Id = "first-girl"
            };
            var secondBoy = new ApplicationUser(DanceGender.Male)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(time)
                {
                    Levels = new List<string> { Level.Beginner }
                },
                Id = "second-boy"
            };
            var secondGirl = new ApplicationUser(DanceGender.Female)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(time)
                {
                    Levels = new List<string> { Level.Beginner }
                },
                Id = "second-girl"
            };
            var thirdBoy = new ApplicationUser(DanceGender.Male)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(time)
                {
                    Levels = new List<string> { Level.Beginner }
                },
                Id = "third-boy"
            };
            var thirdGirl = new ApplicationUser(DanceGender.Female)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(time)
                {
                    Levels = new List<string> { Level.Beginner }
                },
                Id = "third-girl"
            };
            var fourthBoy = new ApplicationUser(DanceGender.Male)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(time)
                {
                    Levels = new List<string> { Level.Beginner }
                },
                Id = "fourth-boy"
            };
            var fourthGirl = new ApplicationUser(DanceGender.Female)
            {
                Levels = new List<string> { Level.Beginner },
                Attending = new Attending(time)
                {
                    Levels = new List<string> { Level.Beginner }
                },
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

            var command = new GetRandomPairingsOfAttendingUsersWithLevelQuery(new List<string> { Level.Beginner });

            var mediator = new Mock<IMediator>();

            mediator
                .Setup(x => x
                    .Send(It.IsAny<GetAllAttendingUsersWithLevelQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(data.ToList()));

            var commandHandler = new GetRandomPairingsOfAttendingUsersWithLevelQueryHandler(mediator.Object);

            //Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            //Assert
            Assert.AreEqual(4, result.Pairings.Count);
            Assert.AreEqual(0, result.LeftoverUsers.Count);
            Assert.AreEqual(fourthGirl.Id, result.Pairings.First(x => x.Male.Id == firstBoy.Id).Female.Id);
            Assert.AreEqual(firstGirl.Id, result.Pairings.First(x => x.Male.Id == secondBoy.Id).Female.Id);
            Assert.AreEqual(thirdGirl.Id, result.Pairings.First(x => x.Male.Id == thirdBoy.Id).Female.Id);
            Assert.AreEqual(secondGirl.Id, result.Pairings.First(x => x.Male.Id == fourthBoy.Id).Female.Id);
        }
    }
}