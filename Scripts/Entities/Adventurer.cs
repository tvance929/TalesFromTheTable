using System;
using System.Collections.Generic;
using TalesFromTheTable.Backgrounds;
using TalesFromTheTable.Scripts.Skills;
using TalesFromTheTable.Skills;
using TalesFromTheTable.Skills.Interfaces;
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
        private Dictionary<AttributeEnum, int> OriginalAttributes;
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
            ResetAttributes();
        }

        public Adventurer(string name)
        {
            Name = name;
            ResetAttributes();

        }

        public bool SetAttributes(Dictionary<AttributeEnum, int> newAbilities)
        {
            //this below will probably not work when changing races, etc
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
            //Set attributes to original values
            foreach (var kvp in OriginalAttributes)
            {
                Attributes[kvp.Key] = kvp.Value;               
            }

            Race = race;

            switch (Race)
            {
                case RaceEnum.Human:
                    //if (humanBonuses.Count != 2) throw new AdventurerException($"Expected 2 bonuses for human, got {humanBonuses.Count}");
                    //Attributes[humanBonuses[0]] += 1;
                    //Attributes[humanBonuses[1]] += 1;
                    //Skills.Add(chosenSkill);
                    //GD.Print($"Human skills: {Skills.Count}"); // ... {Skills[0].Name}");
                    break;
                case RaceEnum.Dwarf:
                    Attributes[AttributeEnum.Constitution] += 1;
                    Attributes[AttributeEnum.Wisdom] += 1;
                    //Set saving throw bonus in AdjustSavingThrowsFromAbilities
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

            AdjustSavingThrowsFromAbilities();

            return race;
        }

        private void AdjustSavingThrowsFromAbilities()
        {
            SavingThrows = new CharacterSavingThrows(); //Reset to 15

            SavingThrows.PoisonOrDeathRay -= Rules.AttributeBonus(Attributes[AttributeEnum.Constitution]);
            SavingThrows.MagicWand -= Rules.AttributeBonus(Attributes[AttributeEnum.Dexterity]);
            SavingThrows.Petrification -= Rules.AttributeBonus(Attributes[AttributeEnum.Intelligence]);
            SavingThrows.DragonBreath -= Rules.AttributeBonus(Attributes[AttributeEnum.Strength]);
            SavingThrows.SpellsOrMagicStaff -= Rules.AttributeBonus(Attributes[AttributeEnum.Wisdom]);

            if (Race == RaceEnum.Dwarf) SavingThrows.PoisonOrDeathRay -= 2;
        }

        /// <summary>
        /// All saving throws start at 15 and will adjust with abilties, race and levels
        /// </summary>
        public class CharacterSavingThrows
        {
            public int PoisonOrDeathRay = 15;
            public int MagicWand = 15;
            public int Petrification = 15;
            public int DragonBreath = 15;
            public int SpellsOrMagicStaff = 15;
        }

        public void SetAttribute(AttributeEnum attribute, int score)
        {
            if (score < 3 || score > 21)
                throw new AdventurerException($"Attempting to set an attribute with a value less than 3 or greater than 21 {attribute} = {score}");
            
            //We need an original set of attributes to have so during creation process, the player can make changes to background and race and not lose the original values
            if (OriginalAttributes[attribute] == 0)
            {
                OriginalAttributes[attribute] = score;
            }

            Attributes[attribute] = score;

            AdjustSavingThrowsFromAbilities();
        }

        public void ResetAttributes()
        {
            Attributes = new Dictionary<AttributeEnum, int> { { AttributeEnum.Strength, 3 }, { AttributeEnum.Dexterity, 3 },
                { AttributeEnum.Constitution, 3 },{ AttributeEnum.Intelligence, 3 }, { AttributeEnum.Wisdom, 3 },
                { AttributeEnum.Charisma, 3 } };
            OriginalAttributes = new Dictionary<AttributeEnum, int> { { AttributeEnum.Strength, 0 }, { AttributeEnum.Dexterity, 0 },
                { AttributeEnum.Constitution, 0 },{ AttributeEnum.Intelligence, 0 }, { AttributeEnum.Wisdom, 0 },
                { AttributeEnum.Charisma, 0 } };
        }

        public void AddSkill(ISkill skill)
        {
            Skills.Add(skill);
        }

        public void SetBackground(Background background)
        {
            //Reset attributes to original values by calling setrace
            SetRace(Race);

            //GD.Print($"background: {background.Name}");
            //Background = background; 

            //GD.Print($"Background: {Background.Name}");
            //Add background bonuses to attributes
            switch (background.Name)
            {
                case "Criminal":
                    Background = new Criminal();
                    Attributes[AttributeEnum.Dexterity] += 1;
                    Attributes[AttributeEnum.Charisma] += 1;
                    Skills.Add(new Deception());
                    break;
                case "Lowborn":
                    Background = new Lowborn();
                    Attributes[AttributeEnum.Strength] += 1;
                    Attributes[AttributeEnum.Constitution] += 1;
                    Skills.Add(new Survival());
                    break;
                case "Noble":
                    Background = new Noble();
                    Attributes[AttributeEnum.Charisma] += 1;
                    Attributes[AttributeEnum.Intelligence] += 1;
                    Skills.Add(new Persuasion());
                    break;
                case "Outlander":
                    Background = new Outlander();
                    Attributes[AttributeEnum.Constitution] += 1;
                    Attributes[AttributeEnum.Wisdom] += 1;
                    Skills.Add(new BeastHandling());
                    break;              
                case "Soldier":
                    Background = new Soldier();
                    Attributes[AttributeEnum.Strength] += 1;
                    Attributes[AttributeEnum.Dexterity] += 1;
                    Skills.Add(new Leadership());
                    break;
                default:
                    break;
            }

            AdjustSavingThrowsFromAbilities();
        }

        public void AttributeAddBonus(AttributeEnum attribute, int bonus)
        {
            Attributes[attribute] += bonus;
        }
    }
}
