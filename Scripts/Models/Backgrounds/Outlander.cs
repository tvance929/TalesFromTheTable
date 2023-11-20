using System.Collections.Generic;
using TalesFromTheTable.Models.Skills;
using TalesFromTheTable.Models.Skills.Interfaces;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Models.Backgrounds
{
    public class Outlander : Background
    {
        public override string Name { get; } = "Outlander";
        public override string Description { get; } = "wilderness or desert...just away from society";
        public override Dictionary<AttributeEnum, int> AbilityBonuses { get; }
            = new Dictionary<AttributeEnum, int> { { AttributeEnum.Wisdom, 1 }, { AttributeEnum.Constitution, 1 } };
        public override List<ISkill> Skills { get; } = new List<ISkill> { new BeastHandling() };
    }
}
