using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationApp.Messaging.Models
{
    public class PreferenceModel
    {
        public List<string> PreferenceOrder { get; set; }

        public string Id { get; set; }

        public PreferenceModel(string id)
        {
            Id = id;
            PreferenceOrder = new List<string>();
        }
    }
}
