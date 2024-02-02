using System.Collections.Generic;
using TalesFromTheTable.Scripts.Utilities.Enums;
using TalesFromTheTable.Utilities;

namespace TalesFromTheTable.Scripts.Models
{
    public class Trap
    {
        public string Description { get; set; }

// 0 for easy now - will have to think how raising the level affects the difficulty and how high we want to go
        public int TrapLevel { get; set; }
        public SaveTypeEnum SaveType { get; set; }  // maybe this isnt needed and they just have to roll a reflex save to see if they dodge
        public List<Die> DamageDice { get; set; }
        public bool Sprung { get; set; } = false;
    }
}