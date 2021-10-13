using System.Collections.Generic;
using Assets.Scripts.Abilities;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Stronghold
{
    public class Cyclops : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>> //todo change swords to clubs
        {
            {EquipLocation.Weapon, new List<string> { "Short Sword", "Sword", "Broad Sword"}}
        };

        public Cyclops() : base(Race.RaceType.Beast, EntityClass.ManAtArms, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Cyclops");

            GenerateStartingEquipment(EntityClass.ManAtArms, _startingEquipmentTable);

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var tank = abilityStore.GetAbilityByName("tank", this);

            Abilities.Add(tank.GetType(), tank);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.monsterHurt;
            DieSound = audioStore.monsterDie;
            AttackSound = audioStore.genericAttack;
        }
    }
}
