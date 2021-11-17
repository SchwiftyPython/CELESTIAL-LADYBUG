using System.Collections.Generic;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Encounters
{
    public class RetreatCombatOption : Option
    {
        public bool Success { get; private set; }
        public List<Entity> Enemies { get; private set; }

        public RetreatCombatOption()
        {

        }

        public RetreatCombatOption(string optionTitle, string optionResultText, List<Entity> enemies, bool success) : base(optionTitle, optionResultText, EncounterType.Combat)
        {
            Enemies = enemies;
            Success = success;
        }
    }
}
