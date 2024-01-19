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
        //if this is true then we wont even look at locked or trapped...still not sure I want to use this.
        public bool Opened { get; set; }

        public bool Locked { get; set; }
        public Trap Trap { get; set; }
        public List<Item> Items { get; set; }

    }
}
