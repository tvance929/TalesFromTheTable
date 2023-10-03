using System;
using System.Collections.Generic;
using TalesFromTheTable.Entities;
using TalesFromTheTable.Utilities.Enums;

namespace TalesFromTheTable.Services.Interfaces
{
    public interface IAdventurerService
    {
        public Dictionary<string, int> RollAbilities (Adventurer adventurer);

        public int ReRollAbility(string abilityRollNumber, Adventurer adventurer);

        public void SetAbilities(List<AbilityEnum> abilityEnums, Adventurer adventurer);
    }
}
