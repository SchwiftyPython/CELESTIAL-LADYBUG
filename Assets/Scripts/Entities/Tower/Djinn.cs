using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Tower
{
    public class Djinn : Entity
    {
        public Djinn() : base(Race.RaceType.Elemental, EntityClass.Wizard, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Djinn");

            //todo magic user

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.monsterHurt;
            DieSound = audioStore.monsterDie;
            AttackSound = audioStore.genericAttack;
        }
    }
}
