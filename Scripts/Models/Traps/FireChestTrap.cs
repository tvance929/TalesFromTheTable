using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalesFromTheTable.Utilities;

namespace TalesFromTheTable.Scripts.Models.Traps
{
    //DONT THINK IM GOING TO USE THESE - RATHER ILL USE TRAP CLASS AND JSON TO CREATE TRAPS for simplicity for now
    public class FireChestTrap : Trap
    {
        public FireChestTrap()
        {
            Description = "A burst of flames engulfs the area in front of the chest, scorching everything in its path.";
            TrapLevel = 2;
            DamageDice = new List<Die> { Die.D6, Die.D6 };
        }
    }
}
