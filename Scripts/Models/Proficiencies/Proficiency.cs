using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Proficiencies
{
    public abstract class Proficiency
    {
        public abstract string Name { get; }
        public abstract ProficiencyTypeEnum ProfType { get; }
        public abstract int Level { get; }
    }
}
