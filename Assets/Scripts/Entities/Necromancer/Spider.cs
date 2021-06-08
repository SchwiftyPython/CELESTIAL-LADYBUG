using Assets.Scripts.Abilities;
using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class Spider : Entity
    {
        public Spider() : base(Race.RaceType.Beast, EntityClass.Beast, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Spider");

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var bite = abilityStore.GetAbilityByName("bite", this);

            Abilities.Add(bite.GetType(), bite);
        }
    }
}
