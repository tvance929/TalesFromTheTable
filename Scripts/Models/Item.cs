using TalesFromTheTable.Models.Items.Interfaces;

namespace TalesFromTheTable.Models.Items
{
    public class Item : IItem
    {
        public ItemType Type;
        public string Name;
        public string Description;
        public int ObjectiveNumber;
        public int Weight; // in lbs
        public double GoldValue;
        public Condition condition = Condition.New;
        public string MetaData; // certain types of items will have a different set of data that needs to be stored
        public string MagicAttributesData; // will need to figure out a system to read each item based on its type to figure out what it does
    }

    public enum ItemType
    {
        Rope,
        Armor,
        Weapon,
        Shield,
        Secret,
        MoneyPile,
        Torch,
        HolySymbol,
        Water,
        Food,
        Tinderbox,
        Key,
        Lockpicks
    }

    public enum Condition
    {
        New,
        Used,
        Worn,
        Ragged,
        Broken
    }
}
