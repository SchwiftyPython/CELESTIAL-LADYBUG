using System.Collections.Generic;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Companions
{
    public class Gladiator : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>>
        {
            {EquipLocation.Weapon, new List<string> {"Broad Sword", "Falchion", "Short Sword"}},
            {EquipLocation.Helmet, new List<string> {"Leather Helmet", "Close Helm"}},
            {EquipLocation.Boots, new List<string> {"Leather Boots", "Worn Leather Boots"}},
            {EquipLocation.Gloves, new List<string> {"Leather Gloves", "Metal Bracers", "Plate Gauntlets", null}},
            {EquipLocation.Shield, new List<string> {"Wooden Shield", "Heater Shield", "Kite Shield", "Knight's Shield"}},
            {EquipLocation.Body, new List<string> {"Plate Armor", "Mail Shirt"}}
        };

        public Gladiator(Race.RaceType rType, bool isPlayer) : base(rType, EntityClass.Gladiator, isPlayer)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Gladiator");

            GenerateStartingEquipment(EntityClass.Gladiator, _startingEquipmentTable);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.companionHurt;
            DieSound = audioStore.companionDie;
        }
    }
}
