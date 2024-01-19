using System.Collections.Generic;
using TalesFromTheTable.Utilities;

namespace TalesFromTheTable.Scripts.Models.Traps
{
    public class PoisonedNeedleChestTrap : Trap
    {
        public PoisonedNeedleChestTrap()
        {
            Description = "A poisoned needle shoots from the chest, attempting to poison those in front of it.";
            TrapLevel = 3;
            DamageDice = new List<Die> { Die.D6, Die.D6 };
        }
    }
}
