using Assets.Scripts.Abilities;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Tower
{
    public class Gargoyle : Entity
    {
        public Gargoyle() : base(Race.RaceType.Beast, EntityClass.Beast, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Gargoyle");

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var clawAttack = abilityStore.GetAbilityByName("claw attack", this);

           AddAbility(clawAttack);

            var demonicIntervention = abilityStore.GetAbilityByName("demonic intervention", this);

            AddAbility(demonicIntervention);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.monsterHurt;
            DieSound = audioStore.monsterDie;
            AttackSound = audioStore.genericAttack;
        }
    }
}
