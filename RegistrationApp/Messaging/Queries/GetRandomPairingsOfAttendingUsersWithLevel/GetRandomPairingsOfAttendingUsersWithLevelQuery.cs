using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MediatR;
using RegistrationApp.Messaging.Models;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Queries.GetRandomPairingsOfAttendingUsersWithLevel
{
    public class GetRandomPairingsOfAttendingUsersWithLevelQuery : IRequest<LevelPairingsModel>
    {
        public List<string> Levels { get; }

        public GetRandomPairingsOfAttendingUsersWithLevelQuery(List<string> levels)
        {
            Levels = levels;
        }
    }
}