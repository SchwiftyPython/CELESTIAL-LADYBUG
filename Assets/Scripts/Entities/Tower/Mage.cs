using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Tower
{
    public class Mage : Entity
    {
        public Mage() : base(Race.RaceType.Human, EntityClass.Wizard, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Mage");

            //todo magic user

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.monsterHurt;
            DieSound = audioStore.monsterDie;
            AttackSound = audioStore.genericAttack;
        }
    }
}
