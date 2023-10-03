using System;
using System.Collections.Generic;
using TalesFromTheTable.Skills;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Backgrounds
{
    public class Lowborn : Background
    {
        public override string Name { get; } = "Lowborn";
        public override string Description { get; } = "school of hard knocks...reilient";
        public override Dictionary<AbilityEnum, int> AbilityBonuses { get; }
            = new Dictionary<AbilityEnum, int> { { AbilityEnum.Strength, 1 }, { AbilityEnum.Constitution, 1 } };
        public override List<Skill> Skills { get; } = new List<Skill> { new Survival() };
    }
}
