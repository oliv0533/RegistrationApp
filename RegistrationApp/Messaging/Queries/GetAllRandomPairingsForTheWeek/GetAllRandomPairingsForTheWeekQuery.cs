using System.Collections.Generic;
using MediatR;
using RegistrationApp.Messaging.Models;

namespace RegistrationApp.Messaging.Queries.GetAllRandomPairingsForTheWeek
{
    public class GetAllRandomPairingsForTheWeekQuery: IRequest<List<LevelPairingsModel>>
    {
        
    }
}