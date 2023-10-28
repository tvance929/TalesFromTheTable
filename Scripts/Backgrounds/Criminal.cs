using System.Collections.Generic;
using TalesFromTheTable.Scripts.Skills;
using TalesFromTheTable.Skills.Interfaces;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Backgrounds
{
    public class Criminal : Background
	{
		public override string Name { get; } = "Criminal";
		public override string Description { get; } = "broke the law and liked it";
		public override Dictionary<AttributeEnum, int> AbilityBonuses { get; }
			= new Dictionary<AttributeEnum, int> { { AttributeEnum.Charisma, 1 }, { AttributeEnum.Dexterity, 1 } };
		public override List<ISkill> Skills { get; } = new List<ISkill> { new Deception() };
	}
}
