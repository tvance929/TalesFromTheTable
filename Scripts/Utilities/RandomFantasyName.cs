using System;
using System.Collections.Generic;

namespace TalesFromTheTable.Utilities
{
    public static class RandomFantasyName
    {
        static readonly Random random = new();

        static readonly List<string> prefixes = new()
        {
            "Ael", "Glo", "Tha", "Zan", "Vor", "Elan", "Thel", "Kyr", "Drae"
        };

        static readonly List<string> suffixes = new()
        {
            "dor", "rion", "las", "miel", "thir", "ynn", "rith", "lor", "nyx"
        };

        static readonly string vowels = "aeiou";
        static readonly string consonants = "bcdfghjklmnpqrstvwxyz";

        static readonly List<string> lastPrefixes = new()
        {
            "Iron", "Stone", "Fire", "Heart", "Blood", "Dragon", "Shadow", "Light", "Steel", "Storm", "Gold", "Silver", "Night", "Thorn", "Winter", "Oak", "Wolf", "Star"
        };

        static readonly List<string> lastSuffixes = new()
        {
            "bane", "fell", "wood", "thorn", "hawk", "shadow", "fire", "frost", "dragon", "moon"
        };

        public static string GenerateFantasyName()
        {
            string prefix = prefixes[random.Next(prefixes.Count)];
            string suffix = suffixes[random.Next(suffixes.Count)];

            char vowel = vowels[random.Next(vowels.Length)];
            char consonant = consonants[random.Next(consonants.Length)];

            string lastName = lastPrefixes[random.Next(lastPrefixes.Count)] + lastSuffixes[random.Next(lastSuffixes.Count)];

            return $"{prefix}{vowel}{consonant}{suffix} {lastName}";
        }
    }
}
