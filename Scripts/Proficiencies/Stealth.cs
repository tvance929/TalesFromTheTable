using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Proficiencies
{
    public class Stealth : Proficiency
    {
        public override string Name { get; } = "Stealth";

        public override ProficiencyType ProfType { get; } = ProficiencyType.Thieving;

        public override int Level { get; } = 1;
    }
}