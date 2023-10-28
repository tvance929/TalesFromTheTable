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

		public const int HIGH_ABILITY = 15; // will not guarentee a higher re-roll over this number

		// First thing should be to roll for abilities
		public Dictionary<string, int> AttributeRolls { get; private set; } = new Dictionary<string, int>();

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
		public Dictionary<string, int> RollAttributes(Adventurer adventurer)
		{
			//if (adventurer.AttributesSet) throw new AdventurerException("This adventurer already has abilties set");
            //if (adventurer.AbilitiesRolled) throw new AdventurerException("This adventurer already has abilties rolled");

            AttributeRolls = new Dictionary<string, int>
            {
                { "one", diceroller.RollDice(threeD6s) },
                { "two", diceroller.RollDice(threeD6s) },
                { "three", diceroller.RollDice(threeD6s) },
                { "four", diceroller.RollDice(threeD6s) },
                { "five", diceroller.RollDice(threeD6s) },
                { "six", diceroller.RollDice(threeD6s) }
            };

            adventurer.AttributesRolled = true;

			return AttributeRolls;
		}

		//Will return 0 if the adventurer cannot reroll anymore abilties
		public int ReRollAbility(string abilityRollNumber, Adventurer adventurer)
		{
			if (!AttributeRolls.ContainsKey(abilityRollNumber))
			{
				throw new Exception($"abilityRollNumber {abilityRollNumber} does not belong to the rolls dictionary");
			}

			if (adventurer.AttributesReRolled <= SystemConstants.AMOUNT_OF_REROLLS_ALLOWED)
			{
				var newRoll = 0;

				// will not guarentee a higher reroll if the ability is already high
				while((AttributeRolls[abilityRollNumber] < HIGH_ABILITY && newRoll <= AttributeRolls[abilityRollNumber]) ||
                    (AttributeRolls[abilityRollNumber] >= HIGH_ABILITY && newRoll < AttributeRolls[abilityRollNumber]))
				{
					newRoll = diceroller.RollDice(threeD6s);
				}	

				AttributeRolls[abilityRollNumber] = newRoll;
				return newRoll;
			}
			else
			{
				return 0;
			}
		}

		//Send in List of ability enums in order of rolls
		public void SetAttributes(List<AttributeEnum> abilityEnums, Adventurer adventurer)
		{
			if (adventurer.AttributesSet)
			{
				throw new Exception("The adventurer's abilities have already been set.");
			}

			var dictionaryPosition = 0;
			var abilities = new Dictionary<AttributeEnum, int>();

			foreach (var abilityEnum in abilityEnums)
			{
				switch (abilityEnum)
				{
					case AttributeEnum.Strength:
						abilities[AttributeEnum.Strength] = AttributeRolls.ElementAt(dictionaryPosition).Value;
						break;
					case AttributeEnum.Dexterity:
						abilities[AttributeEnum.Dexterity] = AttributeRolls.ElementAt(dictionaryPosition).Value;
						break;
					case AttributeEnum.Constitution:
						abilities[AttributeEnum.Constitution] = AttributeRolls.ElementAt(dictionaryPosition).Value;
						break;
					case AttributeEnum.Intelligence:
						abilities[AttributeEnum.Intelligence] = AttributeRolls.ElementAt(dictionaryPosition).Value;
						break;
					case AttributeEnum.Wisdom:
						abilities[AttributeEnum.Wisdom] = AttributeRolls.ElementAt(dictionaryPosition).Value;
						break;
					case AttributeEnum.Charisma:
						abilities[AttributeEnum.Charisma] = AttributeRolls.ElementAt(dictionaryPosition).Value;
						break;
				}

				dictionaryPosition++;
			}

			adventurer.SetAttributes(abilities);
		}
	}
}
