using TalesFromTheTable.Models.Items;

namespace TalesFromTheTable.Scripts.Models.Items
{
    public class ClothArmor : Item
    {
        public ClothArmor()
        {
            Type = ItemType.Armor;
            Name = "Cloth Armor";
            Description = "A piece of armor made from heavy cloth";
            Weight = 2;
            GoldValue = 1;
            condition = Condition.New;
            MetaData = "9"; // armor class
        }
    }

    public class LeatherArmor : Item
    {
        public LeatherArmor()
        {
            Type = ItemType.Armor;
            Name = "Leather Armor";
            Description = "A piece of armor made of leather";
            Weight = 5;
            GoldValue = 5;
            condition = Condition.New;
            MetaData = "11"; // armor class
        }
    }

    public class ChainArmor : Item
    {
        public ChainArmor()
        {
            Type = ItemType.Armor;
            Name = "Chain Armor";
            Description = "A piece of sturdy armor made from interlocking chains";
            Weight = 10;
            GoldValue = 10;
            condition = Condition.New;
            MetaData = "13"; // armor class
        }
    }

    public class PlateArmor : Item
    {
        public PlateArmor()
        {
            Type = ItemType.Armor;
            Name = "Plate Armor";
            Description = "A piece of rugged armor made from plates of metal";
            Weight = 20;
            GoldValue = 25;
            condition = Condition.New;
            MetaData = "15"; // armor class
        }
    }
}
