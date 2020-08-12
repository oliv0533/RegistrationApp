using System.Collections.Generic;
using MediatR;
using RegistrationApp.Messaging.Models;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Queries.GetAllAttendingUsersWithLevel
{
    public class GetAllAttendingUsersWithLevelQuery : IRequest<List<ApplicationUser>>
    {
        public List<Level> Levels { get; }

        public GetAllAttendingUsersWithLevelQuery(List<Level> levels)
        {
            Levels = levels;
        }
    }
}