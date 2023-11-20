using System.Collections.Generic;
using TalesFromTheTable.Models.Entities;
using TalesFromTheTable.Models.Items;

namespace TalesFromTheTable.Models
{
    public class Adventure
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }
        public List<Room> Rooms { get; set; }
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
        public List<Vessel> Vessels { get; set; }
        public List<Exit> Exits { get; set; }
    }
}
