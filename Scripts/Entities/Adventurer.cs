using System;
using System.Collections.Generic;
using TalesFromTheTable.Backgrounds;
using TalesFromTheTable.Proficiencies;
using TalesFromTheTable.Skills;
using TalesFromTheTable.Utilities;
using TalesFromTheTable.Utilities.Enums;
using TalesFromTheTable.Utilities.Exceptions;

namespace TalesFromTheTable.Entities
{
	public class Adventurer : Entity
	{
		// Core
		public string Name;
		public RaceEnum Race { get; private set; } = RaceEnum.NotSet;
		public Background Background { get; private set; }  

		// Physical Abilities
		public Dictionary<AbilityEnum, int> Abilities { get; private set; }
		public CharacterSavingThrows SavingThrows { get; private set; } = new CharacterSavingThrows();
		public int Awareness { get; private set; } = 0;

		// Skills and Crafts
		public List<Skill> Skills { get; private set; } = new List<Skill>();
		public List<Proficiency> Proficiencies { get; private set; } = new List<Proficiency>();

		// Experiences
		public int XP { get; private set; } = 0;
		public List<Guid> AdventuresPlayed { get; private set; } = new List<Guid>();
		public Dictionary<string, int> KillList { get; private set; } = new Dictionary<string, int>();
		public string Obituary { get; private set; } = "";

		// Creation Procedure
		public bool IsCreated = false;
		public bool AbilitiesSet { get; private set; } = false; //set to true when the rolls are applied, cannot apply rolls again
		public int AbilitiesReRolled { get; } = 0;
		public bool SavingThrowsAdjustedFromAbilities = false;


		// ==============================
		// Constructor
		public Adventurer(string name)
		{
			Name = name;
			Abilities = new Dictionary<AbilityEnum, int> { { AbilityEnum.Strength, 0 }, { AbilityEnum.Dexterity, 0 },
				{ AbilityEnum.Constitution, 0 },{ AbilityEnum.Intelligence, 0 }, { AbilityEnum.Wisdom, 0 },
				{ AbilityEnum.Charisma, 0 } };

		}

		public bool SetAbilities(Dictionary<AbilityEnum, int> newAbilities)
		{
			if (AbilitiesSet) return false;

			foreach(var kvp in newAbilities)
			{
				if (kvp.Value < 3 || kvp.Value > 18) 
					throw new AdventurerException($"Attempting to set Abilities on a character with a value less than 3 or greater than 18 {kvp.Key} = {kvp.Value}");
			}

			foreach(var kvp in newAbilities)
			{
				Abilities[kvp.Key] += kvp.Value;
			}

			AbilitiesSet = true;

			AdjustSavingThrowsFromAbilities();

			return true;
		}

		public RaceEnum SetRace(RaceEnum race)
		{
			if (Race != RaceEnum.NotSet) throw new AdventurerException("This adventurer already has their race set");

			Race = race;

			switch (Race)
			{
				case RaceEnum.Human:
					// Pick two attributes to increase 1
					// pick proficiency or skill
					break;
				case RaceEnum.Dwarf:
					Abilities[AbilityEnum.Constitution] += 1;
					Abilities[AbilityEnum.Wisdom] += 1;
					SavingThrows.PoisonOrDeathRay -= 2;
					break;
				case RaceEnum.Elf:
					Abilities[AbilityEnum.Dexterity] += 1;
					Abilities[AbilityEnum.Intelligence] += 1;
					// immune to charm
					break;
				case RaceEnum.Halfling:
					Abilities[AbilityEnum.Dexterity] += 1;
					Abilities[AbilityEnum.Charisma] += 1;
					// Proficient with Lockpick or Stealth
					break;
				default:
					break;
			}

			return race;
		}

		private void AdjustSavingThrowsFromAbilities()
		{
			if (SavingThrowsAdjustedFromAbilities) return;

			SavingThrows.PoisonOrDeathRay -= Rules.AbilityBonus(Abilities[AbilityEnum.Constitution]);
			SavingThrows.MagicWand -= Rules.AbilityBonus(Abilities[AbilityEnum.Dexterity]);
			SavingThrows.Paralysis -= Rules.AbilityBonus(Abilities[AbilityEnum.Intelligence]);
			SavingThrows.DragonBreath -= Rules.AbilityBonus(Abilities[AbilityEnum.Strength]);
			SavingThrows.SpellsOrMagicStaff -= Rules.AbilityBonus(Abilities[AbilityEnum.Wisdom]);

			SavingThrowsAdjustedFromAbilities = true; //only allow this once
		}

		/// <summary>
		/// All saving throws start at 15 and will adjust with abilties, race and levels
		/// </summary>
		public class CharacterSavingThrows
		{
			public int PoisonOrDeathRay = 15;
			public int MagicWand = 15;
			public int Paralysis  = 15;
			public int DragonBreath = 15;
			public int SpellsOrMagicStaff = 15;
		}
	}
}
