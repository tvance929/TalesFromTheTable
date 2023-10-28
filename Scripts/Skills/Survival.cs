using TalesFromTheTable.Skills.Interfaces;

namespace TalesFromTheTable.Scripts.Skills
{
    public class Survival : ISkill
    {
        public string Description { get; } = "Handling of Beasts";
        public string Name { get; } = "Beast Handling";
        public int Level { get; private set; } = 1;

        public void SetLevel(int level)
        {
            Level = level;
        }
    }
}