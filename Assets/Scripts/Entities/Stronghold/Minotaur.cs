using Assets.Scripts.Abilities;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Stronghold
{
    public class Minotaur : Entity
    {
        public Minotaur() : base(Race.RaceType.Beast, EntityClass.Beast, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Minotaur");

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            //todo need boulder throw ability -- would like to animate boulder being thrown

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.monsterHurt;
            DieSound = audioStore.monsterDie;
            AttackSound = audioStore.genericAttack;
        }
    }
}
