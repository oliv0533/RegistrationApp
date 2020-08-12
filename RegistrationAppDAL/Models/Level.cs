using System;

namespace RegistrationAppDAL.Models
{
    public static class Level
    {
        public const string Beginner = "beginner";
        public const string Novice = "novice";
        public const string Advanced = "advanced";
        public const string Theme = "theme";

        public static string[] GetAllLevels()
        {
            return new[] {Beginner, Novice, Advanced, Theme};
        }
    }


}