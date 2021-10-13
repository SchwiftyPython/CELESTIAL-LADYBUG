using Assets.Scripts.Abilities;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Tower
{
    public class Titan : Entity
    {
        public Titan() : base(Race.RaceType.Elemental, EntityClass.Beast, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Titan");

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var rockSlam = abilityStore.GetAbilityByName("crushing blow", this);

            Abilities.Add(rockSlam.GetType(), rockSlam);

            var tank = abilityStore.GetAbilityByName("tank", this);

            Abilities.Add(tank.GetType(), tank);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.monsterHurt;
            DieSound = audioStore.monsterDie;
            AttackSound = audioStore.genericAttack;
        }
    }
}
