using TalesFromTheTable.Models.Skills.Interfaces;
using TalesFromTheTable.Scripts.Utilities.Enums;

namespace TalesFromTheTable.Models.Skills
{
    public class DisarmTrap : ISkill
    {
        public string Description { get; } = "Disarming of Traps";
        public SkillTypeEnum Type { get; } = SkillTypeEnum.DisarmTrap;
        public int Level { get; private set; } = 1;

        public void SetLevel(int level)
        {
            Level = level;
        }
    }
}