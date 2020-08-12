using System;
using System.Dynamic;

namespace RegistrationAppDAL.Models
{
    public class Pairing
    {
        public string Id { get; set; }
        public string MaleUserId { get; set; }

        public string FemaleUserId { get; set; }

        public Pairing(string maleUserId, string femaleUserId)
        {
            MaleUserId = maleUserId;
            FemaleUserId = femaleUserId;
            Id = Guid.NewGuid().ToString();
        }
    }
}