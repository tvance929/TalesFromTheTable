using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalesFromTheTable.Scripts.Utilities
{
    public static class RandomFantasyName
    {
        static List<string> prefixes = new List<string>
        {
            "Ael", "Glo", "Tha", "Zan", "Vor", "Elan", "Thel", "Kyr", "Drae"
        };

        static List<string> suffixes = new List<string>
        {
            "dor", "rion", "las", "miel", "thir", "ynn", "rith", "lor", "nyx"
        };

        static string vowels = "aeiou";
        static string consonants = "bcdfghjklmnpqrstvwxyz";

        static Random random = new Random();

        public static string GenerateFantasyName()
        {
            string prefix = prefixes[random.Next(prefixes.Count)];
            string suffix = suffixes[random.Next(suffixes.Count)];

            char vowel = vowels[random.Next(vowels.Length)];
            char consonant = consonants[random.Next(consonants.Length)];

            return prefix + vowel + consonant + suffix;
        }

        static void Main(string[] args)
        {
            for (int i = 0; i < 5; i++)
            {
                string name = GenerateFantasyName();
                Console.WriteLine(name);
            }
        }
    }
}
