using System;

namespace TalesFromTheTable.Utilities
{
	public static class Rules
	{

		/// <summary>
		/// Bonuses based on ability score
		/// </summary>
		/// <param name="abilityScore"></param>
		/// <returns></returns>
		public static int AbilityBonus(int abilityScore)
		{
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
	}
}
