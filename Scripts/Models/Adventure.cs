﻿using System.Collections.Generic;
using System.Linq;
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
        public List<string> MapArrays { get; set; }

        public void ChestLooted(string roomID)
        {
            var chest = Rooms.Where(r => r.RoomID == roomID).FirstOrDefault().Chest;
            chest.Items.Clear();
        }
    }

    public class Room
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string RoomID { get; set; }

        public List<Item> Items { get; set; }
        public List<Entity> Monsters { get; set; }
        public Trap Trap { get; set; } //one trap per room please
        public List<Door> Doors { get; set; }

        //public List<Vessel> Vessels { get; set; }
        public Chest Chest { get; set; }
        public List<Exit> Exits { get; set; }

        public List<string> RoomDef { get; set; }

        public bool Searched { get; set; } = false;
    }

    //Not using
    //public class MapArrays
    //{
    //    public string Level1 { get; set; }
    //    public string Level2 { get; set; }
    //    public string Level3 { get; set; }
    //}
}
