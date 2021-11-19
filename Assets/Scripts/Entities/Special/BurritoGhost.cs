using Assets.Scripts.Abilities;
using Assets.Scripts.Entities.Necromancer;
using UnityEngine;

namespace Assets.Scripts.Entities.Special
{
    public class BurritoGhost : Ghost
    {
        public BurritoGhost()
        {
            Name = "Burrito Ghost";

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var fart = abilityStore.GetAbilityByName("fart", this);

            AddAbility(fart);

            //todo make a little tougher than normal ghosts
        }
    }
}
