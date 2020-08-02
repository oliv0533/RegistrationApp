using System.Linq;
using RegistrationAppDAL.Models;

namespace RegistrationAppTests
{
    public class TestHelper
    {
        public static void SetupData(IQueryable<ApplicationUser> data, UnitTestHandle testHandle)
        {
            testHandle.MockSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(data.Provider);
            testHandle.MockSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(data.Expression);
            testHandle.MockSet.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            testHandle.MockSet.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }
    }
}