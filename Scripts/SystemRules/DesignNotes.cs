namespace TalesFromTheTable.Scripts.SystemRules
{
    internal class DesignNotes
    {
        //ACTIONS -- these should be the options that can occur within a room... the json file should tell the game engine what is available
            //Take Exits -- do this first to allow player to move around the map
            //Combat - once engaged
            //Sneak
            //Search
            //Disarm - if trap found
            //Picklock
            //Open Chest
            //Talk - this will be a whole different thing I think where each discussion will have multiple outcomes based on what is done

        //OTHER ACTIONS
            //Inventory
            //Drink Potion
            //Eat Food

        //KEYWORDS - so if a thing is done that important to the adventure storyline a keyword will be given and added to the adventurer
        // this can then be referenced in other rooms such as "if player has keyword X then do this" and "if player has keyword Y and X then do this"
    }
}
