using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using RegistrationAppDAL.Data;
using RegistrationAppDAL.Models;

namespace RegistrationAppTests
{
    public class UnitTestHandle : IDisposable
    {
        public Mock<ApplicationDbContext> MockContext { get; set; }

        public Mock<DbSet<ApplicationUser>> MockSet { get; set; }

        public UnitTestHandle()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
            MockContext = new Mock<ApplicationDbContext>(options);

            MockSet = new Mock<DbSet<ApplicationUser>>();

            MockContext.Setup(m => m.Users).Returns(MockSet.Object);
        }

        public void Dispose()
        {
        }
    }
}