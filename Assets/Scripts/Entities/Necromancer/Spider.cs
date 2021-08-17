using Assets.Scripts.Abilities;
using Assets.Scripts.Audio;
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

            //todo a web ability?
            //at least a few other generic spider attacks

            var bite = abilityStore.GetAbilityByName("bite", this);

            Abilities.Add(bite.GetType(), bite);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.spiderHurt;
            DieSound = audioStore.spiderDie;
            AttackSound = audioStore.spiderAttack;
        }
    }
}
