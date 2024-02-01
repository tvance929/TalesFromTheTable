using TalesFromTheTable.Models.Skills.Interfaces;
using TalesFromTheTable.Scripts.Utilities.Enums;

namespace TalesFromTheTable.Models.Skills
{
    public class Lockpick : ISkill
    {
        public string Description { get; } = "Picking of locks";
        public SkillTypeEnum Type { get; } = SkillTypeEnum.Lockpick;
        public int Level { get; private set; } = 1;

        public void SetLevel(int level)
        {
            Level = level;
        }
    }
}