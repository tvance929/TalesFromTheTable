using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalesFromTheTable.Entities;

namespace TalesFromTheTable.Scripts.SystemServices
{
    public static class GameService
    {
        public static Adventurer Adventurer { get; private set; }
        public static string playerLocation;

        public static void StartGame(Adventurer adventurer)
        {
            Adventurer = adventurer;

        }
    }
}
