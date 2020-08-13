using System.Collections.Generic;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Models
{
    public class UserResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public UserResponseModel? Match { get; set; }

        public List<string> Levels { get; set; }

        public string Gender { get; set; }


        public UserResponseModel(string id, string name, string email, string phoneNumber, string gender)
        {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Gender = gender;
            Levels = new List<string>();
        }
    }
}