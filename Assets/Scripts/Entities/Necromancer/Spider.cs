using Assets.Scripts.Abilities;
using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class Spider : Entity
    {
        private static readonly string _hurtSound = GlobalHelper.MonsterHitOne;
        private static readonly string _dieSound = GlobalHelper.MonsterDieOne;

        public Spider() : base(Race.RaceType.Beast, EntityClass.Beast, false, _hurtSound, _dieSound)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Spider");

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            //todo a web ability?
            //at least a few other generic spider attacks

            var bite = abilityStore.GetAbilityByName("bite", this);

            Abilities.Add(bite.GetType(), bite);
        }
    }
}
