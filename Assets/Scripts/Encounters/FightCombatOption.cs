using System.Collections.Generic;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Encounters
{
    public class FightCombatOption : Option
    {
        public List<Entity> Enemies { get; private set; }

        public FightCombatOption(string optionTitle, string optionResultText, List<Entity> enemies) : base(optionTitle, optionResultText)
        {
            Enemies = enemies;
        }
    }
}
