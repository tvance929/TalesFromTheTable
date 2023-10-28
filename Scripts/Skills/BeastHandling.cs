using TalesFromTheTable.Skills.Interfaces;

namespace TalesFromTheTable.Skills
{
    public class BeastHandling : ISkill
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
