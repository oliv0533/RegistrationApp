using System;
using System.Collections.Generic;
using System.Linq;
using RegistrationApp.Messaging.Models;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Services
{
    public class EnumConverterService : IEnumConverterService
    {
        public string ConvertLevelToDanishString(string level)
        {
            return level switch
            {
                Level.Beginner => Constants.BeginnerDanish,
                Level.Novice => Constants.NoviceDanish,
                Level.Advanced => Constants.AdvancedDanish,
                Level.Theme => Constants.ThemeDanish,
                _ => throw new InvalidOperationException("Enum not found")
            };
        }

        public string ConvertDanishStringToLevel(string level)
        {
            return level switch
            {
                Constants.BeginnerDanish => Level.Beginner,
                Constants.NoviceDanish => Level.Novice,
                Constants.AdvancedDanish => Level.Advanced,
                Constants.ThemeDanish => Level.Theme,
                _ => throw new InvalidOperationException("Enum not found")
            };
        }

        public string ConvertGenderToDanishString(string gender)
        {
            return gender switch
            {
                DanceGender.Male => Constants.MaleDanish,
                DanceGender.Female => Constants.FemaleDanish,
                _ => throw new InvalidOperationException("Enum not found")
            };
        }

        public string ConvertDanishStringToGender(string gender)
        {
            return gender switch
            {
                Constants.MaleDanish => DanceGender.Male,
                Constants.FemaleDanish => DanceGender.Female,
                _ => throw new InvalidOperationException("Enum not found")
            };
        }

        public List<string> GetAllGendersInDanish()
        {
            return (from gender in (DanceGender.GetAllDanceGenders()) select ConvertGenderToDanishString(gender)).ToList();
        }

    }

    public interface IEnumConverterService
    {
        public string ConvertLevelToDanishString(string level);
        public string ConvertDanishStringToLevel(string level);
        public string ConvertGenderToDanishString(string gender);
        public string ConvertDanishStringToGender(string gender);
        public List<string> GetAllGendersInDanish();

    }
}