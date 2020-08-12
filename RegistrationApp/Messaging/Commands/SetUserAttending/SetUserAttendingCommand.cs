using System;
using System.Collections.Generic;
using MediatR;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Commands.SetUserAttending
{
    public class SetUserAttendingCommand: IRequest
    {
        public string UserId { get; set; }

         public Attending? Attending { get; set; }

        public SetUserAttendingCommand(string userId)
        {
            UserId = userId;
        }
    }
}