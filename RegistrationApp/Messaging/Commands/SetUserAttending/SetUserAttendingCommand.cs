using System;
using MediatR;

namespace RegistrationApp.Messaging.Commands.SetUserAttending
{
    public class SetUserAttendingCommand: IRequest
    {
        public string UserId { get; set; }

        public DateTime? Attending { get; set; }

        public SetUserAttendingCommand(string userId)
        {
            UserId = userId;
        }
    }
}