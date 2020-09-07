using System.Collections.Generic;
using System.Security.Cryptography;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Commands.UpdateUser
{
    public class UpdateUserDetailsCommand : IRequest
    {
        public string UserId { get; set; }
        public List<string> Levels { get; set; }
        public string Gender { get; set; }

        public UpdateUserDetailsCommand(string userId, List<string> levels, string gender)
        {
            UserId = userId;
            Levels = levels;
            Gender = gender;
        }
    }
}