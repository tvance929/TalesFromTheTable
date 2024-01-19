using System.Collections.Generic;
using TalesFromTheTable.Utilities;

namespace TalesFromTheTable.Scripts.Models.Traps
{
    public class PoisonGasChestTrap : Trap
    {
        public PoisonGasChestTrap()
        {
            Description = "A poisonous gas is released in front of the chest, affecting those who trigger it.";
            TrapLevel = 5;
            DamageDice = new List<Die> { Die.D10, Die.D10 };
        }
    }
}
