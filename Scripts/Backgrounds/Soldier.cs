using System;
using System.Collections.Generic;
using TalesFromTheTable.Skills;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Backgrounds
{
    public class Soldier : Background
    {
        public override string Name { get; } = "Soldier";
        public override string Description { get; } = "disciplined and experienced in combat";
        public override Dictionary<AbilityEnum, int> AbilityBonuses { get; }
            = new Dictionary<AbilityEnum, int> { { AbilityEnum.Strength, 1 }, { AbilityEnum.Dexterity, 1 } };
        public override List<Skill> Skills { get; } = new List<Skill> { new Leadership() };
    }
}
