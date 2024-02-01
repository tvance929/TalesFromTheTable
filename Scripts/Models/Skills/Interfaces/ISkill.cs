using TalesFromTheTable.Scripts.Utilities.Enums;

namespace TalesFromTheTable.Models.Skills.Interfaces
{
    public interface ISkill
    {
        public SkillTypeEnum Type { get; }
        public string Description { get; }
        public int Level { get; }
    }
}
