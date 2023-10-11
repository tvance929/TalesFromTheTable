using System;
using System.Collections.Generic;
using System.Linq;
using TalesFromTheTable.Entities;
using TalesFromTheTable.Services.Interfaces;
using TalesFromTheTable.Utilities;
using TalesFromTheTable.Utilities.Enums;
using TalesFromTheTable.Utilities.Exceptions;

namespace TalesFromTheTable.Services
{
	public class AdventurerService : IAdventurerService
	{
		// Dice for rolling
		private Dice diceroller;
		private readonly List<Die> threeD6s = new List<Die> { Die.D6, Die.D6, Die.D6 };

		// First thing should be to roll for abilities
		public Dictionary<string, int> AbilityRolls { get; private set; } = new Dictionary<string, int>();

		public AdventurerService(Dice diceRoller)
		{
			this.diceroller = diceRoller;
		}

		/// <summary>
		/// This should be the first step after creating an adventurer.
		/// </summary>
		/// <param name="adventurer"></param>
		/// <returns></returns>
		/// <exception cref="AdventurerException"></exception>
		public Dictionary<string, int> RollAbilities(Adventurer adventurer)
		{
			if (adventurer.AbilitiesSet) throw new AdventurerException("This adventurer already has abilties set");
			//if (adventurer.AbilitiesRolled) throw new AdventurerException("This adventurer already has abilties rolled");

			AbilityRolls = new Dictionary<string, int>();
			AbilityRolls.Add("one", diceroller.RollDice(threeD6s));
			AbilityRolls.Add("two", diceroller.RollDice(threeD6s));
			AbilityRolls.Add("three", diceroller.RollDice(threeD6s));
			AbilityRolls.Add("four", diceroller.RollDice(threeD6s));
			AbilityRolls.Add("five", diceroller.RollDice(threeD6s));
			AbilityRolls.Add("six", diceroller.RollDice(threeD6s));

			adventurer.AbilitiesRolled = true;

			return AbilityRolls;
		}

		//Will return 0 if the adventurer cannot reroll anymore abilties
		public int ReRollAbility(string abilityRollNumber, Adventurer adventurer)
		{
			if (!AbilityRolls.ContainsKey(abilityRollNumber))
			{
				throw new Exception($"abilityRollNumber {abilityRollNumber} does not belong to the rolls dictionary");
			}

			if (adventurer.AbilitiesReRolled <= SystemConstants.AMOUNT_OF_REROLLS_ALLOWED)
			{
				var newRoll = 0;

				while(newRoll <= AbilityRolls[abilityRollNumber])
				{
					newRoll = diceroller.RollDice(threeD6s);
				}	

				AbilityRolls[abilityRollNumber] = newRoll;
				return newRoll;
			}
			else
			{
				return 0;
			}
		}

		//Send in List of ability enums in order of rolls
		public void SetAbilities(List<AbilityEnum> abilityEnums, Adventurer adventurer)
		{
			if (adventurer.AbilitiesSet)
			{
				throw new Exception("The adventurer's abilities have already been set.");
			}

			var dictionaryPosition = 0;
			var abilities = new Dictionary<AbilityEnum, int>();

			foreach (var abilityEnum in abilityEnums)
			{
				switch (abilityEnum)
				{
					case AbilityEnum.Strength:
						abilities[AbilityEnum.Strength] = AbilityRolls.ElementAt(dictionaryPosition).Value;
						break;
					case AbilityEnum.Dexterity:
						abilities[AbilityEnum.Dexterity] = AbilityRolls.ElementAt(dictionaryPosition).Value;
						break;
					case AbilityEnum.Constitution:
						abilities[AbilityEnum.Constitution] = AbilityRolls.ElementAt(dictionaryPosition).Value;
						break;
					case AbilityEnum.Intelligence:
						abilities[AbilityEnum.Intelligence] = AbilityRolls.ElementAt(dictionaryPosition).Value;
						break;
					case AbilityEnum.Wisdom:
						abilities[AbilityEnum.Wisdom] = AbilityRolls.ElementAt(dictionaryPosition).Value;
						break;
					case AbilityEnum.Charisma:
						abilities[AbilityEnum.Charisma] = AbilityRolls.ElementAt(dictionaryPosition).Value;
						break;
				}

				dictionaryPosition++;
			}

			adventurer.SetAbilities(abilities);
		}
	}
}
