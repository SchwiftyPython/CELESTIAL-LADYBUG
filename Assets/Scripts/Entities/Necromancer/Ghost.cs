using Assets.Scripts.Abilities;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class Ghost : Entity
    {
        public Ghost() : base(Race.RaceType.Undead, EntityClass.Ethereal, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Ghost");

            //todo maybe some creepy boi abilities
            //pass through obstacles
            //buff other undead
            //possession

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var intimidate = abilityStore.GetAbilityByName("intimidate", this);

            AddAbility(intimidate);

            var spookyTouch = abilityStore.GetAbilityByName("spooky touch", this);

            AddAbility(spookyTouch);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.monsterHurt;
            DieSound = audioStore.monsterDie;
            AttackSound = audioStore.genericAttack;
        }
    }
}
