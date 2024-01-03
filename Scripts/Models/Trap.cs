using System.Collections.Generic;
using TalesFromTheTable.Scripts.Utilities.Enums;
using TalesFromTheTable.Utilities;

namespace TalesFromTheTable.Models
{
    public class Trap
    {
        public TrapType Type { get; set; }
        public bool Sprung { get; set; } = false;
        public List<Die> DamageDice { get; set; }
    }
}