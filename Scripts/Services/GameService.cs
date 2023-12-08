using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Newtonsoft.Json;
using TalesFromTheTable.Models;
using TalesFromTheTable.Models.Entities;

namespace TalesFromTheTable.SystemServices
{
    public static class GameService
    {
        private const string DEFAULT_ADVENTURE_NAME = "ShadowsBelowTheMarsh";

        
        public static string AdventureName = DEFAULT_ADVENTURE_NAME;
        public static Adventurer Adventurer { get; private set; }
        public static Adventure Adventure { get; private set; }
        public static event Action AdventureLoaded;

        public static int PlayerLevel = 0;
        public static int PlayerRoom = 0;
        public static string PlayerLocation { get; private set; } = "0-0";

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
            var roomImagePath = $"{AdventureName}/Assets/images/rooms/{PlayerLocation}.jpg";

            return Path.Combine(adventuresFolderPath, roomImagePath);
        }

        public static List<Exit> CurrentRoomExits()
        {
            return Adventure.Rooms.Where(r => r.RoomID == PlayerLocation).FirstOrDefault().Exits;
        }

        public static void StartAdventure()
        {
            PlayerLocation = "1-1";
        }
    }
}
