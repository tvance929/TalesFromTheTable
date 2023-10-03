using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Proficiencies
{
    public class Lockpick : Proficiency
    {
        public override string Name { get; } = "Lockpick";

        public override ProficiencyType ProfType { get; } = ProficiencyType.Thieving;

        public override int Level { get; } = 1;
    }
}