using System.IO;
using Godot;
using Newtonsoft.Json;
using TalesFromTheTable.Models.Entities;

namespace TalesFromTheTable.SystemServices
{
    public partial class SaveService
    {
        const string SAVE_GAME_PATH = "user//";
        const string SAVE_GAME_EXTENSION = ".tftt";

        int version = 1;

        public SaveService() { }

        public void SaveGame(Adventurer adventurer, string saveName = "")
        {
            string filePath = $"{SAVE_GAME_PATH}{adventurer.Name}{SAVE_GAME_EXTENSION}";

            if (saveName != "")
            {
                filePath = $"{SAVE_GAME_PATH}{saveName}{SAVE_GAME_EXTENSION}";
            }

            GD.Print($"Saving to {filePath}");

            File.WriteAllText(filePath, JsonConvert.SerializeObject(adventurer));
        }
    }
}
