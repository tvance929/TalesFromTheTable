using System;
using System.Collections.Generic;
using TalesFromTheTable.Backgrounds;
using TalesFromTheTable.Scripts.Skills;
using TalesFromTheTable.Utilities;
using TalesFromTheTable.Utilities.Enums;
using TalesFromTheTable.Utilities.Exceptions;

namespace TalesFromTheTable.Entities
{
    public class Adventurer : Entity
    {
        // Core
        public string Name;
        public RaceEnum Race { get; private set; } // = RaceEnum.NotSet;
        public Background Background { get; private set; }

        // Physical Abilities
        public Dictionary<AttributeEnum, int> Attributes { get; private set; }
        public CharacterSavingThrows SavingThrows { get; private set; } = new CharacterSavingThrows();
        public int Awareness { get; private set; } = 0;

        // Skills and Crafts
        //public List<ISkill> Skills { get; private set; } = new List<ISkill>();
        //public List<Proficiency> Proficiencies { get; private set; } = new List<Proficiency>();

        // Experiences
        public int XP { get; private set; } = 0;
        public List<Guid> AdventuresPlayed { get; private set; } = new List<Guid>();
        public Dictionary<string, int> KillList { get; private set; } = new Dictionary<string, int>();
        public string Obituary { get; private set; } = "";

        // Creation Procedure
        public bool IsCreated = false;
        public bool AttributesSet { get; private set; } = false; //set to true when the rolls are applied, cannot apply rolls again
        public bool AttributesRolled = false;
        public int AttributesReRolled { get; } = 0;
        public bool SavingThrowsAdjustedFromAbilities = false;


        // ==============================
        // Constructor
        public Adventurer()
        {
            Attributes = new Dictionary<AttributeEnum, int> { { AttributeEnum.Strength, 0 }, { AttributeEnum.Dexterity, 0 },
                                            { AttributeEnum.Constitution, 0 },{ AttributeEnum.Intelligence, 0 }, { AttributeEnum.Wisdom, 0 },
                                            { AttributeEnum.Charisma, 0 } };
        }

        public Adventurer(string name)
        {
            Name = name;
            Attributes = new Dictionary<AttributeEnum, int> { { AttributeEnum.Strength, 0 }, { AttributeEnum.Dexterity, 0 },
                { AttributeEnum.Constitution, 0 },{ AttributeEnum.Intelligence, 0 }, { AttributeEnum.Wisdom, 0 },
                { AttributeEnum.Charisma, 0 } };

        }

        public bool SetAttributes(Dictionary<AttributeEnum, int> newAbilities)
        {
            if (AttributesSet) return false;

            foreach (var kvp in newAbilities)
            {
                if (kvp.Value < 3 || kvp.Value > 18)
                    throw new AdventurerException($"Attempting to set Abilities on a character with a value less than 3 or greater than 18 {kvp.Key} = {kvp.Value}");
            }

            foreach (var kvp in newAbilities)
            {
                Attributes[kvp.Key] += kvp.Value;
            }

            AttributesSet = true;

            AdjustSavingThrowsFromAbilities();

            return true;
        }

        public RaceEnum SetRace(RaceEnum race)
        {
          //  if (Race != RaceEnum.NotSet) throw new AdventurerException("This adventurer already has their race set");

            Race = race;

            switch (Race)
            {
                case RaceEnum.Human:
                    // Pick two attributes to increase 1
                    // pick proficiency or skill
                    break;
                case RaceEnum.Dwarf:
                    Attributes[AttributeEnum.Constitution] += 1;
                    Attributes[AttributeEnum.Wisdom] += 1;
                    SavingThrows.PoisonOrDeathRay -= 2;
                    break;
                case RaceEnum.Elf:
                    Attributes[AttributeEnum.Dexterity] += 1;
                    Attributes[AttributeEnum.Intelligence] += 1;
                    // immune to charm
                    break;
                case RaceEnum.Halfling:
                    Attributes[AttributeEnum.Dexterity] += 1;
                    Attributes[AttributeEnum.Charisma] += 1;
                    Skills.Add(new Lockpick());
                    Skills.Add(new Stealth());
                    break;
                default:
                    break;
            }

            return race;
        }

        private void AdjustSavingThrowsFromAbilities()
        {
            if (SavingThrowsAdjustedFromAbilities) return;

            SavingThrows.PoisonOrDeathRay -= Rules.AbilityBonus(Attributes[AttributeEnum.Constitution]);
            SavingThrows.MagicWand -= Rules.AbilityBonus(Attributes[AttributeEnum.Dexterity]);
            SavingThrows.Paralysis -= Rules.AbilityBonus(Attributes[AttributeEnum.Intelligence]);
            SavingThrows.DragonBreath -= Rules.AbilityBonus(Attributes[AttributeEnum.Strength]);
            SavingThrows.SpellsOrMagicStaff -= Rules.AbilityBonus(Attributes[AttributeEnum.Wisdom]);

            SavingThrowsAdjustedFromAbilities = true; //only allow this once
        }

        /// <summary>
        /// All saving throws start at 15 and will adjust with abilties, race and levels
        /// </summary>
        public class CharacterSavingThrows
        {
            public int PoisonOrDeathRay = 15;
            public int MagicWand = 15;
            public int Paralysis = 15;
            public int DragonBreath = 15;
            public int SpellsOrMagicStaff = 15;
        }

        public void SetAttribute(AttributeEnum attribute, int score)
        {
            if (score < 3 || score > 21)
                throw new AdventurerException($"Attempting to set an attribute with a value less than 3 or greater than 21 {attribute} = {score}");

            Attributes[attribute] = score;
        }
    }
}
