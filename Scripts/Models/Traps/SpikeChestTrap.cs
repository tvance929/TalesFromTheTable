using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalesFromTheTable.Utilities;

namespace TalesFromTheTable.Scripts.Models.Traps
{
    public class SpikeChestTrap : Trap
    {
        public SpikeChestTrap()
        {
            Description = "Sharp spikes shoot up from the chest, causing piercing damage to those in front of it.";
            TrapLevel = 1;
            DamageDice = new List<Die> { Die.D4, Die.D4 };
        }
    }
}
