using System;
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

        private static string adventureName = DEFAULT_ADVENTURE_NAME;

        public static Adventurer Adventurer { get; private set; }
        public static Adventure Adventure { get; private set; }
        public static event Action AdventureLoaded;

        public static int PlayerLevel = 1;
        public static int PlayerRoom = 1;

        public static bool SkippingCreation = false; //for dev purposes only

        public static void StartGame(Adventurer adventurer, bool customAdventure = false, string customAdventureFileName = "")
        {
            Adventurer = adventurer;

            adventureName = customAdventure ? customAdventureFileName : DEFAULT_ADVENTURE_NAME; //make sure to STRIP json off end of customAdventureName in case
            var adventureFilePath = $"{adventureName}/{adventureName}.json";
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
            //test waiting a few seconds here to see if its a timing thing
            var currentLocation = $"{PlayerLevel}-{PlayerRoom}";

            GD.Print($"get message  {Adventure.Rooms[0].RoomID} is this equal to {currentLocation}");
            var roomText = Adventure.Rooms.Where(r => r.RoomID == currentLocation).FirstOrDefault().Description;
            return roomText;
        }

        public static string CurrentRoomImageUrl()
        {
            var adventuresFolderPath = ProjectSettings.GlobalizePath("res://Adventures/");
            var roomImagePath = $"{adventureName}/Assets/{PlayerLevel}-{PlayerRoom}.jpg";

            return Path.Combine(adventuresFolderPath, roomImagePath);
        }
    }
}
