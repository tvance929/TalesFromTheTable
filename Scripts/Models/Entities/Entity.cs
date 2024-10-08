using System.Collections.Generic;
using TalesFromTheTable.Models.Items;
using TalesFromTheTable.Models.Skills.Interfaces;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Models.Entities
{
    public abstract class Entity
	{
		public int Hitpoints { get; protected set; } = 0;
		public int Gold { get; protected set; } = 0;
		public int Level { get; protected set; } = 1;
		public bool IsAlive { get; protected set; } = true;
		public int ArmorClass { get; protected set; } = 0;
		public List<Item> Inventory { get; protected set; } = new List<Item>();
		public List<LanguageEnum> Languages { get; protected set; } = new List<LanguageEnum>();
		public List<ISkill> Skills { get; protected set; } = new List<ISkill>();

		public class Attack
		{
			public int BaseDie { get; protected set; }
			public int BonusDamage { get; protected set; }
		}
	}
}
