using Assets.Scripts.Abilities;
using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class Vampire : Entity
    {
        public Vampire() : base(Race.RaceType.Undead, EntityClass.Wizard, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Vampire");

            //todo need some spells boy-o

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var vBite = abilityStore.GetAbilityByName("vampire bite", this);

            Abilities.Add(vBite.GetType(), vBite);

            var intimidate = abilityStore.GetAbilityByName("intimidate", this);

            Abilities.Add(intimidate.GetType(), intimidate);
        }
    }
}
