using System.Collections.Generic;
using TalesFromTheTable.Utilities;

namespace TalesFromTheTable.Scripts.Models.Traps
{
    public class LightningChestTrap : Trap
    {
        public LightningChestTrap()
        {
            Description = "A bolt of lightning strikes in front of the chest, targeting those who dared to open it.";
            TrapLevel = 4;
            DamageDice = new List<Die> { Die.D8, Die.D8 };
        }
    }
}
