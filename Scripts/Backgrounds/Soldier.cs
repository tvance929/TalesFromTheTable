using System.Collections.Generic;
using TalesFromTheTable.Scripts.Skills;
using TalesFromTheTable.Skills.Interfaces;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Backgrounds
{
	public class Soldier : Background
	{
		public override string Name { get; } = "Soldier";
		public override string Description { get; } = "disciplined and experienced in combat";
		public override Dictionary<AttributeEnum, int> AbilityBonuses { get; }
			= new Dictionary<AttributeEnum, int> { { AttributeEnum.Strength, 1 }, { AttributeEnum.Dexterity, 1 } };
		public override List<ISkill> Skills { get; } = new List<ISkill> { new Leadership() };
	}
}
