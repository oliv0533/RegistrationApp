﻿using System;
using RegistrationApp.Messaging.Models;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Services
{
    public class EnumConverterService : IEnumConverterService
    {
        public string ConvertLevelToDanishString(Level level)
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

        public Level ConvertDanishStringToLevel(string level)
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

        public string ConvertGenderToDanishString(DanceGender gender)
        {
            return gender switch
            {
                DanceGender.Male => Constants.MaleDanish,
                DanceGender.Female => Constants.FemaleDanish,
                _ => throw new InvalidOperationException("Enum not found")
            };
        }

        public DanceGender ConvertDanishStringToGender(string gender)
        {
            return gender switch
            {
                Constants.MaleDanish => DanceGender.Male,
                Constants.FemaleDanish => DanceGender.Female,
                _ => throw new InvalidOperationException("Enum not found")
            };
        }
    }

    public interface IEnumConverterService
    {
        public string ConvertLevelToDanishString(Level level);
        public Level ConvertDanishStringToLevel(string level);
        public string ConvertGenderToDanishString(DanceGender gender);
        public DanceGender ConvertDanishStringToGender(string gender);

    }
}