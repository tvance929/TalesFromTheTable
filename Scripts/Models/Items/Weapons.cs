using TalesFromTheTable.Models.Items;

namespace TalesFromTheTable.Scripts.Models.Items
{
    public class Dagger : Item
    {
        public Dagger()
        {
            Type = ItemType.Weapon;
            Name = "Dagger";
            Description = "...";
            Weight = 1;
            GoldValue = 2;
            condition = Condition.New;
            MetaData = "4,0,1"; // damage die, plus damage, hands used
        }
    }

    public class Shortsword : Item
    {
        public Shortsword()
        {
            Type = ItemType.Weapon;
            Name = "Shortsword";
            Description = "do this later...";
            Weight = 3;
            GoldValue = 3;
            condition = Condition.New;
            MetaData = "6,0,1"; // damage die, plus damage, hands used
        }
    }
}
