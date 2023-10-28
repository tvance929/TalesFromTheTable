using System.Collections.Generic;
using TalesFromTheTable.Scripts.Skills;
using TalesFromTheTable.Skills.Interfaces;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Backgrounds
{
	public class Noble : Background
	{
		public override string Name { get; } = "Noble";
		public override string Description { get; } = "refined and intelligent";
		public override Dictionary<AttributeEnum, int> AbilityBonuses { get; }
			= new Dictionary<AttributeEnum, int> { { AttributeEnum.Charisma, 1 }, { AttributeEnum.Intelligence, 1 } };
		public override List<ISkill> Skills { get; } = new List<ISkill> { new Persuasion() };
	}
}
