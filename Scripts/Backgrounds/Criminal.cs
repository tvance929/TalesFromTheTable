using System;
using System.Collections.Generic;
using TalesFromTheTable.Skills;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Backgrounds
{
	public class Criminal : Background
	{
		public override string Name { get; } = "Criminal";
		public override string Description { get; } = "broke the law and liked it";
		public override Dictionary<AbilityEnum, int> AbilityBonuses { get; }
			= new Dictionary<AbilityEnum, int> { { AbilityEnum.Charisma, 1 }, { AbilityEnum.Dexterity, 1 } };
		public override List<Skill> Skills { get; } = new List<Skill> { new Deception() };
	}
}
