namespace MageNPCTracker.Models
{
    public class ImbuedView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SpellName { get; set; }

        public short Cost { get; set; }

        public short Mana { get; set; }

        public short? BatteryDots { get; set; }
    }
}
