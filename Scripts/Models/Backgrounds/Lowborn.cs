using System.Collections.Generic;
using TalesFromTheTable.Models.Skills;
using TalesFromTheTable.Models.Skills.Interfaces;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Models.Backgrounds
{
    public class Lowborn : Background
    {
        public override string Name { get; } = "Lowborn";
        public override string Description { get; } = "school of hard knocks...reilient";
        public override Dictionary<AttributeEnum, int> AbilityBonuses { get; }
            = new Dictionary<AttributeEnum, int> { { AttributeEnum.Strength, 1 }, { AttributeEnum.Constitution, 1 } };
        public override List<ISkill> Skills { get; } = new List<ISkill> { new Survival() };
    }
}
