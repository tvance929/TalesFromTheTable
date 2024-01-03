using static Godot.Image;
using static Godot.TextServer;
using System.Net.Sockets;
using System.Runtime.Intrinsics.X86;
using System;
using TalesFromTheTable.Models;

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


        //1-2-2024
        //OK so rather than have a SEARCH function for each room - we will roll an awareness check each time they enter a room... if there is a trap in the room and they are not aware of it and then they make an action they set the trap off.
        //So I would need to know which way they entered from... if they back out of the room and go in the direction they came, then maybe we dont have the trap fire.
        //ALSO - we could use this mechanic for finding secret exits...
        //they should only get one chance so we need a 'searched' boolean on the room... so if they failed to find the trap and door they cannot find it (passively) again.  maybe theres a skill they can use later on for a second try.
        //Either way...forget all this search loot stuff and move on to chests - this is taking too much time.
        //I am not even sure if I like the idea of room traps and secret doors... at least not yet.
        //If and when we do do this though, we can report that we are rolling secret awareness check... to give the player a better D&D feel.
    }
}
