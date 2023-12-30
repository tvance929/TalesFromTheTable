using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Newtonsoft.Json;
using TalesFromTheTable.Models;
using TalesFromTheTable.Models.Entities;
using TalesFromTheTable.Scripts.Utilities.Enums;
using TalesFromTheTable.Utilities;

namespace TalesFromTheTable.SystemServices
{
    public static class GameService
    {
        private const string DEFAULT_ADVENTURE_NAME = "ShadowsBelowTheMarsh";


        public static string AdventureName = DEFAULT_ADVENTURE_NAME;
        public static Adventurer Adventurer { get; private set; }
        public static Adventure Adventure { get; private set; }
        public static event Action AdventureLoaded;

        private static int PlayerLevelIndex = 0;
        //public static int PlayerRoom = 0;
        public static string PlayerLocation { get; private set; } = "0-0";
        public static Room currentRoom { get; private set; }

        public static bool SkippingCreation = false; //for dev purposes only

        public static void StartGame(Adventurer adventurer, bool customAdventure = false, string customAdventureFileName = "")
        {
            Adventurer = adventurer;

            AdventureName = customAdventure ? customAdventureFileName : DEFAULT_ADVENTURE_NAME; //make sure to STRIP json off end of customAdventureName in case
            var adventureFilePath = $"{AdventureName}/{AdventureName}.json";
            var adventuresFolderPath = ProjectSettings.GlobalizePath("res://Adventures/");

            var fullPath = Path.Combine(adventuresFolderPath, adventureFilePath);
            //GD.Print($"{fullPath}");

            var jsonContent = File.ReadAllText(fullPath);
            Adventure = JsonConvert.DeserializeObject<Adventure>(jsonContent);
            //GD.Print($"start  {Adventure}");

            AdventureLoaded?.Invoke();

            //Need to show title and description of adventure in main text box first ... then show room description
        }

        public static string GetRoomMessage()
        {
            GD.Print($"get message  {Adventure.Rooms[0].RoomID} is this equal to {PlayerLocation}");
            var roomText = Adventure.Rooms.Where(r => r.RoomID == PlayerLocation).FirstOrDefault().Description;
            return roomText;
        }

        public static string CurrentRoomImageUrl()
        {
            var adventuresFolderPath = ProjectSettings.GlobalizePath("res://Adventures/");
            var roomImagePath = $"{AdventureName}/Assets/images/rooms/{PlayerLocation}.png";

            return Path.Combine(adventuresFolderPath, roomImagePath);
        }

        public static List<Exit> CurrentRoomExits()
        {
            return Adventure.Rooms.Where(r => r.RoomID == PlayerLocation).FirstOrDefault().Exits;
        }

        public static void MovePlayer(ActionsEnum action)
        {
            currentRoom = Adventure.Rooms.Where(r => r.RoomID == PlayerLocation).FirstOrDefault();
            var exit = currentRoom.Exits.Where(e => e.directionAction == action).FirstOrDefault();
            if (exit != null)
            {
                PlayerLocation = exit.roomID;
            }
        }

        public static void StartAdventure()
        {
            PlayerLocation = "1-1";
            PlayerLevelIndex = 0;  // 0 for level 1
            currentRoom = Adventure.Rooms.Where(r => r.RoomID == PlayerLocation).FirstOrDefault();
        }

        public static List<string> ReturnCurrentMapArray()
        {
            return Adventure.MapArrays[PlayerLevelIndex].Split(',').Select(s => s.Trim()).ToList();
        }



        public static string SearchRoom()
        {
            if (currentRoom.Trap != null)
            {
                //did they notice the trap? - their awareness score will be 10 + (con, wis, int) bonuses
                var awarenessCheck = new Dice().RollDice(new List<Die> { Die.D20 });
                if (awarenessCheck >= Adventurer.Awareness)
                {
                    return "You notice a trap!";
                }
                else
                {
                    return "You did not notice a trap!";
                }

                //if (currentRoom.Trap.Sprung)
                //{
                //    return "You notice a trap, but it has already been sprung!";
                //}
                //else
                //{
                //    //did they notice the trap?
                //    if (Adventurer.SavingThrows.PoisonOrDeathRay >= currentRoom.Trap.Difficulty)
                //    {
                //        return "You notice a trap!";
                //    }
                //    else
                //    {
                //        return "You did not notice a trap!";
                //    }
                //}

            }

            //Check if there is loot to be had - check if its trapped - check if its locked - check if its empty - send back a string as the status.
            if (currentRoom.Items.Count > 0)
            {

                foreach (var item in currentRoom.Items)
                {

                }
                return "Looted the room!";
                //if (loot.IsTrapped)
                //{
                //    return "The chest is trapped!";
                //}
                //else if (loot.IsLocked)
                //{
                //    return "The chest is locked!";
                //}
                //else
                //{
                //    return "You found some loot!";
                //}
            }
            else
            {
                return "There is nothing to loot here!";
            }

        }

        internal static bool CurrentRoomHasLootOrActiveTraps()
        {
           return (currentRoom.Items.Count > 0 || (currentRoom.Trap != null && !currentRoom.Trap.Sprung));
        }

        internal static bool CurrentRoomHasLootableChest()
        {
            return (currentRoom.Chest != null && !currentRoom.Chest.Looted);
        }
    }
}
