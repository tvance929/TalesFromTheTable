using System.Collections.Generic;
using TalesFromTheTable.Scripts.Utilities.Enums;
using TalesFromTheTable.Utilities;

namespace TalesFromTheTable.Scripts.Models
{
    public class Trap
    {
        public string Description { get; set; }
        //Dont think Ill need this since we have save type 
        public int TrapLevel { get; set; }
        public SaveTypeEnum SaveType { get; set; }
        public List<Die> DamageDice { get; set; }
        public bool Sprung { get; set; } = false;
    }
}