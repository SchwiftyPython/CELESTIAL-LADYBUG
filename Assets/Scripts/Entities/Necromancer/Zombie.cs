using Assets.Scripts.Abilities;
using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class Zombie : Entity
    {
        public Zombie() : base(Race.RaceType.Undead, EntityClass.ManAtArms, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Zombie");

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var eEndurance = abilityStore.GetAbilityByName("endangered endurance", this);

            Abilities.Add(eEndurance.GetType(), eEndurance);
        }
    }
}
