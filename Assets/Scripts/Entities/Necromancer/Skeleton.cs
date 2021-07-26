﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class Skeleton : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>>
        {
            {EquipLocation.Weapon, new List<string> { "Short Sword", "Sword", "Broad Sword"}},
            {EquipLocation.Helmet, new List<string> {"Leather Helmet", null}},
            {EquipLocation.Boots, new List<string> {"Worn Leather Boots", null}},
            {EquipLocation.Body, new List<string> {"Worn Mail Shirt", null}}
        };

        private static readonly string _hurtSound = "event:/Skeleton Hit"; //todo might need decorator for the fmod event
        private static readonly string _dieSound = "event:/Skeleton Hit";

        public Skeleton() : base(Race.RaceType.Undead, EntityClass.ManAtArms, false, _hurtSound, _dieSound)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Skeleton");

            GenerateStartingEquipment(EntityClass.ManAtArms, _startingEquipmentTable);
        }
    }
}
