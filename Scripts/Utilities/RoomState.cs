namespace TalesFromTheTable.Scripts.Utilities
{
    /// <summary>
    /// This will represent what the player knows of the current state of the current room
    /// I dont need even half of these probably, there are only a few things I will need to 
    /// use to tell the UI to turn on or off buttons
    /// </summary>
    public static class RoomState
    {
        public static string RoomDescription { get; set; } = string.Empty;
        public static bool Visited { get; set; } = false;
        public static bool HasChest { get; set; } = false;
        public static bool ChestLocked { get; set; } = false;
        public static bool ChestTrapped { get; set; } = false;
        public static bool ChestTrapSprung { get; set; } = false;
        public static bool ChestOpened { get; set; } = false;
        //public bool TrapDiscovered { get; set; } = false;
        //public bool TrapSprung { get; set; } = false;
        //public bool HasMonster { get; set; } = false;
        //public bool MonsterDefeated { get; set; } = false;
        //public bool HasTreasure { get; set; } = false;
        //public bool TreasureTaken { get; set; } = false;
        //public bool HasExit { get; set; } = false;
        //public bool ExitLocked { get; set; } = false;
        //public bool ExitTrapped { get; set; } = false;
        //public bool ExitOpened { get; set; } = false;
        //public bool HasStairs { get; set; } = false;
        //public bool StairsLocked { get; set; } = false;
        //public bool StairsTrapped { get; set; } = false;
        //public bool StairsOpened { get; set; } = false;
        //public bool HasSecretDoor { get; set; } = false;
        //public bool SecretDoorLocked { get; set; } = false;
        //public bool SecretDoorTrapped { get; set; } = false;
        //public bool SecretDoorOpened { get; set; } = false;
        //public bool HasFountain { get; set; } = false;
        //public bool FountainUsed { get; set; } = false;
        //public bool HasPit { get; set; } = false;
        //public bool PitCrossed { get; set; } = false;
        //public bool HasTeleporter { get; set; } = false;
        //public bool TeleporterUsed { get; set; } = false;
        //public bool HasMagicPool { get; set; } = false;
        //public bool MagicPoolUsed { get; set; } = false;
        //public bool HasMagicItem { get; set; } = false;
        //public bool MagicItemTaken { get; set; } = false;
        //public bool HasMagicWeapon { get; set; } = false;
        //public bool MagicWeaponTaken { get; set; } = false;
        //public bool HasMagicArmor { get; set; } = false;
        //public bool MagicArmorTaken { get; set; } = false;
        //public bool HasMagicRing { get; set; } = false;
        //public bool MagicRingTaken { get; set; } = false;
        //public bool HasMagicScroll { get; set; } = false;
        //public bool MagicScrollTaken { get; set; } = false;
        //public bool HasMagicWand { get; set; } = false;
        //public bool MagicWandTaken { get; set; } = false;
        //public bool HasMagicPotion { get; set; } = false;            
        //public bool MagicPotionTaken { get; set; } = false;
        //public bool HasMagicBook { get; set; } = false;
        //public bool MagicBookTaken { get; set; } = false;        

        public static void ResetState()
        {
            RoomDescription = string.Empty;
            Visited = false;
            // Reset other properties...
        }
    }
}
