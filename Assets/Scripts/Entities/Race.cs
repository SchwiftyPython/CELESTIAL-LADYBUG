using System;
using System.ComponentModel;

namespace Assets.Scripts.Entities
{
    public class Race : IModifierProvider
    {
        [ES3Serializable] private readonly RaceType _rType;

        //todo does it matter if the types are not represented graphically?
        public enum RaceType
        {
            [Description("Human")] Human,
            [Description("Dwarf")] Dwarf,
            [Description("Elf")] Elf,
            [Description("Gnome")] Gnome,
            [Description("Halfling")] Halfling,
            [Description("Undead")] Undead,
            [Description("Beast")] Beast,
            [Description("Derpus")] Derpus,
            [Description("Elemental")] Elemental
        }

        public Race()
        {
        }

        public Race(RaceType rType)
        {
            _rType = rType;
        }

        public RaceType GetRaceType()
        {
            return _rType;
        }

        public float GetAdditiveModifiers(Enum stat)
        {
            throw new NotImplementedException();
        }

        public float GetPercentageModifiers(Enum stat)
        {
            throw new NotImplementedException();
        }
    }
}
