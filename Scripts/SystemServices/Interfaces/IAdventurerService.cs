using System;
using System.Collections.Generic;
using TalesFromTheTable.Entities;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Services.Interfaces
{
    public interface IAdventurerService
    {
        public Dictionary<string, int> RollAttributes (Adventurer adventurer);

        public int ReRollAbility(string abilityRollNumber, Adventurer adventurer);

        public void SetAttributes(List<AttributeEnum> abilityEnums, Adventurer adventurer);
    }
}
