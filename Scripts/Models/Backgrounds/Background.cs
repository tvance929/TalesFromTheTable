using System.Collections.Generic;
using TalesFromTheTable.Models.Skills.Interfaces;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Models.Backgrounds
{
    public abstract class Background
	{
		public abstract string Name { get; }
		public abstract string Description { get; }
		public abstract Dictionary<AttributeEnum, int> AbilityBonuses { get; }
		public abstract List<ISkill> Skills { get; }
	}
}
