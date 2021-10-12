using System.Collections.Generic;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Stronghold
{
    public class Wolfrider : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>> //todo swords to axes
        {
            {EquipLocation.Weapon, new List<string> { "Short Sword", "Sword", "Broad Sword"}},
            {EquipLocation.Helmet, new List<string> {"Leather Helmet", null}},
            {EquipLocation.Boots, new List<string> {"Worn Leather Boots", null}},
            {EquipLocation.Body, new List<string> {"Worn Mail Shirt", null}}
        };

        public Wolfrider() : base(Race.RaceType.Beast, EntityClass.ManAtArms, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Wolfrider");

            GenerateStartingEquipment(EntityClass.ManAtArms, _startingEquipmentTable);

            //todo some kind of mount charge ability

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.monsterHurt;
            DieSound = audioStore.monsterDie;
            AttackSound = audioStore.genericAttack;
        }
    }
}
