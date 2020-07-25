using System.Collections.Generic;
using MediatR;
using RegistrationApp.Messaging.Models;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Queries.GetAllUsersFromDatabase
{
    public class GetAllUsersWithLevelQuery : IRequest<List<UserResponseModel>>
    {
        public DanceClass Level { get; }

        public GetAllUsersWithLevelQuery(DanceClass level)
        {
            Level = level;
        }
    }
}