namespace TalesFromTheTable.Models.Skills.Interfaces
{
    public interface ISkill
    {
        public string Name { get; }
        public string Description { get; }
        public int Level { get; }
    }
}
