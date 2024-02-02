using System;
using System.Collections.Generic;
using TalesFromTheTable.Models.Entities;

namespace TalesFromTheTable.Utilities
{
    public static class Rules
	{

		/// <summary>
		/// Bonuses based on attribute score
		/// </summary>
		/// <param name="abilityScore"></param>
		/// <returns></returns>
		public static int AttributeBonus(int abilityScore)
		{
			//GD.Print(abilityScore);
			switch (abilityScore)
			{
				case 3:
					return -3;
				case int i when i > 3 && i < 6:
					return -2;
				case int i when i > 5 && i < 9:
					return -1;
				case int i when i > 8 && i < 13:
					return 0;
				case int i when i > 12 && i < 16:
					return 1;
				case int i when i > 15 && i < 18:
					return 2;
				case int i when i > 17 && i < 21:
					return 3;
				case int i when i > 20 && i < 23:
					return 4;
				default:
					throw new Exception($"abilityScore is outside of the range {abilityScore}");
			}
		}

		/// <summary>
		/// Standard traps will be 13 DC 
		/// Player gets a +5 for having disarm skill making it a 8 DC ( 65% ) chance
		/// Will decide later how trap level affects difficulty
		/// </summary>
		/// <param name="trapLevel"></param>
		/// <param name="hasDisarmSkill"></param>
		/// <returns></returns>
		public static int ReturnTrapDC(int trapLevel, bool hasDisarmSkill)
		{
			return hasDisarmSkill ? 8 : 13;		
		}


		/// <summary>
		/// Standard Reflex save is 15 - attribute bonus
		/// </summary>
		/// <param name="charactersDexterity"></param>
		/// <returns></returns>
		public static int ReturnReflexSave(int charactersDexterity)
		{

			return 15 - AttributeBonus(charactersDexterity);
		}
	}
}
