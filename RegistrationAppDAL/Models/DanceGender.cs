using Microsoft.VisualBasic.CompilerServices;

namespace RegistrationAppDAL.Models
{
    public static class DanceGender
    {
        public const string Male = "male";
        public const string Female = "female";

        public static string[] GetAllDanceGenders()
        {
            return new [] {Male, Female};
        }
    }
}