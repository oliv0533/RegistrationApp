using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MediatR;
using RegistrationApp.Messaging.Models;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Queries.GetRandomPairingsOfAttendingUsersWithLevel
{
    public class GetRandomPairingsOfAttendingUsersWithLevelQuery : IRequest<List<UserResponseModel>>
    {
        public List<string> Levels { get; }

        public GetRandomPairingsOfAttendingUsersWithLevelQuery(List<string> levels)
        {
            Levels = levels;
        }
    }
}