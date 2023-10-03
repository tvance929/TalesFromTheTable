using System;
using System.Collections.Generic;
using TalesFromTheTable;
using TalesFromTheTable.Skills;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Backgrounds
{
	public abstract class Background
	{
		public abstract string Name { get; }
		public abstract string Description { get; }
		public abstract Dictionary<AbilityEnum, int> AbilityBonuses { get; }
		public abstract List<Skill> Skills { get; }
	}
}
