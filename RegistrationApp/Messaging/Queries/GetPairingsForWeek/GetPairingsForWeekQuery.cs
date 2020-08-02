using System;
using System.Collections.Generic;
using MediatR;
using RegistrationApp.Messaging.Models;

namespace RegistrationApp.Messaging.Queries.GetPairingsForWeek
{
    public class GetPairingsForWeekQuery : IRequest<List<UserResponseModel>>
    {
        public DateTime Date { get; set; }
    }
}