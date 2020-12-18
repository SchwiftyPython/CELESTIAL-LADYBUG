using System.Collections.Generic;

namespace Assets.Scripts.Encounters
{
    public abstract class Encounter
    {
        public Rarity Rarity;

        public EncounterType EncounterType;

        public string Title;

        public string Description;

        public List<Option> Options;

        public abstract void Run();

        public bool HasOptions()
        {
            return Options != null && Options.Count > 0;
        }
    }
}
