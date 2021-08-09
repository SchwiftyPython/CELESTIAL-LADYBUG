using Assets.Scripts.Abilities;
using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class Ghost : Entity
    {
        private static readonly string _hurtSound = GlobalHelper.MonsterHitOne;
        private static readonly string _dieSound = GlobalHelper.MonsterDieOne;

        public Ghost() : base(Race.RaceType.Undead, EntityClass.Ethereal, false, _hurtSound, _dieSound)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Ghost");

            //todo some creepy boi abilities
            //pass through obstacles
            //buff other undead
            //possession
            //generic ghost attack

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var intimidate = abilityStore.GetAbilityByName("intimidate", this);

            Abilities.Add(intimidate.GetType(), intimidate);
        }
    }
}
