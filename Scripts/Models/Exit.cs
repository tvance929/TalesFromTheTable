using TalesFromTheTable.Scripts.Utilities.Enums;

namespace TalesFromTheTable.Models
{
    public class Exit
    {
        public string roomID { get; set; }
        public ActionsEnum directionAction { get; set; }
    }
}