using System.IO;
using Godot;
using Newtonsoft.Json;
using TalesFromTheTable.Models.Entities;
using TalesFromTheTable.Models;
using System;
using Godot.NativeInterop;

namespace TalesFromTheTable.SystemServices
{
    public static class GameService
    {
        private const string DEFAULT_ADVENTURE_NAME = "ShadowsBelowTheMarsh";
        public static Adventurer Adventurer { get; private set; }
        public static string playerLocation;

        public static void StartGame(Adventurer adventurer, bool customAdventure = false, string customAdventureFileName = "")
        {
            Adventurer = adventurer;

            var adventureFile = customAdventure ? customAdventureFileName : DEFAULT_ADVENTURE_NAME; //make sure to STRIP json off end of customAdventureName in case
            var adventuresFolderPath = ProjectSettings.GlobalizePath("res://Adventures/");
            var fullPath = Path.Combine(adventuresFolderPath, adventureFile + ".json");
            var jsonContent = File.ReadAllText(fullPath);
            var adventure = JsonConvert.DeserializeObject<Adventure>(jsonContent);

            GD.Print($"rooms count {adventure.Rooms.Count} - {adventure.Rooms[0].Exits[0].direction} -- {adventure.Rooms[0].Items[0].Name}");
        }

        public static string GetRoomMessage()
        {
            return "You are in a room.";
        }
    }
}
