using System.Collections.Generic;
using TalesFromTheTable.Models.Items;
using TalesFromTheTable.Scripts.Models.Interfaces;

namespace TalesFromTheTable.Scripts.Models
{
    /// <summary>
    /// There may be a better way to represent this but rather than it being a vessel, I am thinking a basic "chest" and one per room 
    /// </summary>
    public class Chest : IVessel
    {
        public bool IsLocked { get; set; }
        public bool IsTrapped { get; set; }
        public bool Opened { get; set; }
        public bool Looted { get; set; }
        public List<Item> Items { get; set; }
    }
}
