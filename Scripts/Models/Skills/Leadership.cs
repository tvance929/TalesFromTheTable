﻿using TalesFromTheTable.Models.Skills.Interfaces;
using TalesFromTheTable.Scripts.Utilities.Enums;

namespace TalesFromTheTable.Models.Skills
{
    public class Leadership : ISkill
    {
        public string Description { get; } = "Handling of Beasts";
        public SkillTypeEnum Type { get; } = SkillTypeEnum.Leadership;
        public int Level { get; private set; } = 1;

        public void SetLevel(int level)
        {
            Level = level;
        }
    }
}
