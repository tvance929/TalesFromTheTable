namespace TalesFromTheTable.System
{
    internal class CharacterRules
    {

        // *** ROLLING ATTRIBUTES
        // When making adventurer, player makes six d6 rolls and can re-roll two of those -- taking the higher of the two.  
        // Player is then allowed to assign values to character.
        // No classes - do what you want, why not.  Skills can be gained and used by every adventurer
        // Although - make an adv too even then maybe you cant do something you want in the future OR too specialized... what to do?


        //in our system service we are rolling the dice all at once - it may be more fun for a user in the future to do one roll at a time.. dont know
        //maybe too laborious that way - but rolling 3d dice per ability could be fun .... 

        // Creation steps
        // 1 - Make a name - ctor
        // 2 - Roll for abilities - before a background because they might choose a different background based on their attr rolls
        // 3 - Pick a Background and Race - these can happen interchangebly as decisions are made
        // 4 - Choose a craft  ( pick locks, cast spells, say prayers, heavy armor, weapon training ) ... these will give you a bonus to these things but players will still be able to try these things without the craft
        //          also - these can have levels .. Heavy Armor 2 .. more bonuses or more armor with bonus,  pick locks 2 could allow picking for more complicated locks or just better bonuses

        // for now all classes use a d8 for hit points and every level will be a maximum for HP + cons.... or should they roll?  Might be more fun to force them to roll but also frustrating - could 
        // keep track of their rolls for each level as a memory for fun?

    }
}
