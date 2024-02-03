using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Newtonsoft.Json;
using TalesFromTheTable.Models;
using TalesFromTheTable.Models.Entities;
using TalesFromTheTable.Models.Items;
using TalesFromTheTable.Scripts.Models;
using TalesFromTheTable.Scripts.Utilities;
using TalesFromTheTable.Scripts.Utilities.Enums;
using TalesFromTheTable.Utilities;
using TalesFromTheTable.Utilities.Exceptions;
using static System.Net.Mime.MediaTypeNames;

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
        public static bool TrapDiscovered { get; set; } = false;

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

        public static List<string> GetRoomDefinition(string roomID)
        {
            GD.Print($"get message  {Adventure.Rooms[0].RoomID} is this equal to {PlayerLocation}");
            var roomDef = Adventure.Rooms.Where(r => r.RoomID == roomID).FirstOrDefault().RoomDef;
            return roomDef;
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
            //RESET ROOM STATE HERE 

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

        public static void OpenChest()
        {
            RoomState.RoomDescription = "\n";

            var chest = currentRoom.Chest;

            //double check this logic... dont think a chest will be opened and there will be loot in it ... until we make a way for players to drop items.  
            if (chest.Opened)
            {
                RoomState.RoomDescription += $"[b] The chest is opened...[/b]\n";
                //RoomState.RoomDescription += $"[b][color=green]You found some loot![/color][/b]\n";
                //chest.Opened = true;
                //RoomState.ChestOpened = true; // this is probably unnecessary - but just setting it for now

                if (chest.Items.Count > 0)
                {
                    RoomState.RoomDescription += $"[b]THERE ARE ITEMS INSIDE THE CHEST: [/b]\n";
                    LootChest();
                }
            }
            else if (chest.Trap != null)
            {
                //did they notice the trap? - their awareness score will be 10 + (con, wis, int) bonuses
                var awarenessCheck = new Dice().RollDice(new List<Die> { Die.D20 });
                if (Adventurer.Awareness <= awarenessCheck)
                {
                    RoomState.RoomDescription += $"[b][color=red]You discover that the chest is TRAPPED![/color][/b]\n";
                    RoomState.ChestTrapped = true;
                }
                else
                {
                    SpringTrap(chest.Trap);
                }
            }
            else if (chest.Locked)
            {
                //do picklock 
            }
            else
            {
                RoomState.RoomDescription += $"You open the chest. \n";
                if (chest.Items.Count > 0)
                {
                    RoomState.RoomDescription += $"[b]THERE ARE ITEMS INSIDE THE CHEST: [/b]\n";
                    LootChest();
                }
            }
        }

        public static void DisarmTrap()
        {
            RoomState.RoomDescription = "\n";

            var trap = currentRoom.Chest.Trap;

            var hasDisarmSkill = Adventurer.Skills.Any(skill => skill.Type == SkillTypeEnum.DisarmTrap);

            var trapDC = Rules.ReturnTrapDC(trap.TrapLevel, hasDisarmSkill);
            var trapDCRoll = new Dice().RollDice(new List<Die> { Die.D20 });
            var disarmedTrap = trapDCRoll  >= trapDC;

            RoomState.RoomDescription += $"( Trap DC : {trapDC} and you rolled a {trapDCRoll} ) \n";

            if (disarmedTrap)
            {
                RoomState.RoomDescription += $"You disarmed the trap! Phew!!!";
            }
            else
            {
                SpringTrap(trap);
            }           
       
            if (Adventurer.Hitpoints > 0)
            {
                LootChest();
            }
        }

        public static void PickLock()
        {

        }

        private static void SpringTrap(Trap trap)
        {
            RoomState.RoomDescription += "You tripped the trap!!  Rolling a reflex save! \n";

            var reflexSaveDC = Rules.ReturnReflexSave(Adventurer.Attributes.Where(a => a.Key == Utilities.Enums.AttributeEnum.Dexterity).FirstOrDefault().Value);
            var reflexSaveRoll = new Dice().RollDice(new List<Die> { Die.D20 });
            var saved = reflexSaveRoll >= reflexSaveDC;

            RoomState.RoomDescription += $"( Reflex DC : {reflexSaveDC} and you rolled a {reflexSaveDC} ) \n";

            if (saved)
            {
                RoomState.RoomDescription += "However you were able to dodge the deadly device! Phew! \n";
            }
            else
            {
                RoomState.RoomDescription += "You could not get clear of the trap!  Rolling Damage... \n";

                var damage = Adventurer.TakeDamage(trap.DamageDice);

                RoomState.RoomDescription += $"You take {damage} damage.  And you're hit points are now {Adventurer.Hitpoints}. \n";

                if (Adventurer.Hitpoints < 1)
                {
                    RoomState.RoomDescription += $"aaannnddd you're dead. \n";
                }
            }

            trap.Sprung = true;
        }

        private static void LootChest()
        {
            // add whatever is in chest to inventory and report.
            RoomState.RoomDescription += $"You loot the chest and receive : \n";

            foreach (Item i in currentRoom.Chest.Items)
            {
                Adventurer.Inventory.Add(i);
                RoomState.RoomDescription += $"{i.Name} \n";
            }

            Adventure.ChestLooted(currentRoom.RoomID);
        }

        public static string SearchRoom()
        {
            var returnMessage = "\n";

            if (currentRoom.Trap != null)
            {
                var trap = currentRoom.Trap;
                //did they notice the trap? - their awareness score will be 10 + (con, wis, int) bonuses
                var awarenessCheck = new Dice().RollDice(new List<Die> { Die.D20 });
                if (Adventurer.Awareness <= awarenessCheck && trap.Sprung == false)
                {
                    returnMessage += $"[b][color=red]You notice that the chest is trapped![/color][/b]\n";
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
            //if (currentRoom.Items.Count > 0)
            //{

            //    foreach (var item in currentRoom.Items)
            //    {

            //    }
            //    return "Looted the room!";
            //    //if (loot.IsTrapped)
            //    //{
            //    //    return "The chest is trapped!";
            //    //}
            //    //else if (loot.IsLocked)
            //    //{
            //    //    return "The chest is locked!";
            //    //}
            //    //else
            //    //{
            //    //    return "You found some loot!";
            //    //}
            //}
            //else
            //{
            //    return "There is nothing to loot here!";
            //}

            return returnMessage;
        }

        internal static bool CurrentRoomHasLootOrActiveTraps()
        {
            return (currentRoom.Items.Count > 0 || (currentRoom.Trap != null && !currentRoom.Trap.Sprung));
        }

        internal static bool CurrentRoomHasLootableChest()
        {
            return (currentRoom.Chest != null && currentRoom.Chest.Items.Count > 0);
        }
    }
}
