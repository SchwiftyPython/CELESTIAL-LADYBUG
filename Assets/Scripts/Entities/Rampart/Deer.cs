using Assets.Scripts.Abilities;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Rampart
{
    public class Deer : Entity
    {
        public Deer() : base(Race.RaceType.Beast, EntityClass.Beast, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Deer");

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var hoofAttack = abilityStore.GetAbilityByName("hoof slap", this);

            AddAbility(hoofAttack);

            var naturesBlessing = abilityStore.GetAbilityByName("natures blessing", this);

            AddAbility(naturesBlessing);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.monsterHurt;
            DieSound = audioStore.monsterDie;
            AttackSound = audioStore.genericAttack;
        }
    }
}
