using System;
using System.Collections.Generic;
using TalesFromTheTable.Skills;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Backgrounds
{
    public class Noble : Background
    {
        public override string Name { get; } = "Noble";
        public override string Description { get; } = "refined and intelligent";
        public override Dictionary<AbilityEnum, int> AbilityBonuses { get; }
            = new Dictionary<AbilityEnum, int> { { AbilityEnum.Charisma, 1 }, { AbilityEnum.Intelligence, 1 } };
        public override List<Skill> Skills { get; } = new List<Skill> { new Persuasion() };
    }
}
