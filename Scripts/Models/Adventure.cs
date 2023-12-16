using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TalesFromTheTable.Models.Entities;
using TalesFromTheTable.Models.Items;
using TalesFromTheTable.Scripts.Models;

namespace TalesFromTheTable.Models
{
    public class Adventure
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }
        public List<Room> Rooms { get; set; }        
        public MapArrays MapArrays { get; set; }
    }

    public class Room
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string RoomID { get; set; }

        public List<Item> Items { get; set; }
        public List<Entity> Monsters { get; set; }
        public List<Trap> Traps { get; set; }
        public List<Door> Doors { get; set; }

        //public List<Vessel> Vessels { get; set; }
        public Chest Chest { get; set; }
        public List<Exit> Exits { get; set; }
    }

    public class MapArrays
    {
        public string Level1 { get; set; }
        public string Level2 { get; set; }
        public string Level3 { get; set; }
    }
}
