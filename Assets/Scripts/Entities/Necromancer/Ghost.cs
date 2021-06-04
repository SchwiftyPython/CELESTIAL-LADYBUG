using Assets.Scripts.Abilities;
using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class Ghost : Entity
    {
        public Ghost() : base(Race.RaceType.Undead, EntityClass.Ethereal, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Ghost");

            //todo some creepy boi abilities
            //pass through obstacles
            //buff other undead
            //possession

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var intimidate = abilityStore.GetAbilityByName("intimidate", this);

            Abilities.Add(intimidate.GetType(), intimidate);
        }
    }
}
