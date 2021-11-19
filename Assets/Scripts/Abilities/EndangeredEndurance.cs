using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class EndangeredEndurance : Ability
    {
        private bool _savedFromDeath;

        public EndangeredEndurance(Entity abilityOwner) : base("Endangered Endurance", "Once per combat, if health falls to zero or below, set health to one.", -1, -1, abilityOwner, TargetType.Friendly, true)
        {
            _savedFromDeath = false;
        }

        public bool SavedFromDeathThisBattle()
        {
            return _savedFromDeath;
        }

        public override int Use()
        {
            if (SavedFromDeathThisBattle())
            {
                return -1;
            }

            AbilityOwner.Stats.CurrentHealth = 1;

            _savedFromDeath = true;

            return -1;
        }

        public void Reset()
        {
            _savedFromDeath = false;
        }
    }
}
