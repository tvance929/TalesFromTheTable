using System;
using System.Collections.Generic;
using TalesFromTheTable.Skills;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Backgrounds
{
    public class Outlander : Background
    {
        public override string Name { get; } = "Outlander";
        public override string Description { get; } = "wilderness or desert...just away from society";
        public override Dictionary<AbilityEnum, int> AbilityBonuses { get; }
            = new Dictionary<AbilityEnum, int> { { AbilityEnum.Wisdom, 1 }, { AbilityEnum.Constitution, 1 } };
        public override List<Skill> Skills { get; } = new List<Skill> { new BeastHandling() };
    }
}
