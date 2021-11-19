using Assets.Scripts.Abilities;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Stronghold
{
    public class Harpy : Entity
    {
        public Harpy() : base(Race.RaceType.Beast, EntityClass.Beast, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Harpy");

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var clawAttack = abilityStore.GetAbilityByName("claw attack", this);

            AddAbility(clawAttack);

            var uncannyDodge = abilityStore.GetAbilityByName("uncanny dodge", this);

            AddAbility(uncannyDodge);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.monsterHurt;
            DieSound = audioStore.monsterDie;
            AttackSound = audioStore.genericAttack;
        }
    }
}
