using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Companions
{
    public class Knight : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>>
        {
            {EquipLocation.Weapon, new List<string> { "Warbrand"}},
            {EquipLocation.Helmet, new List<string> {"Leather Helmet", "Close Helm"}},
            {EquipLocation.Boots, new List<string> {"Leather Boots", "Worn Leather Boots"}},
            {EquipLocation.Gloves, new List<string> {"Leather Gloves", "Metal Bracers", "Plate Gauntlets", null}},
            {EquipLocation.Body, new List<string> {"Plate Armor", "Mail Shirt"}}
        };

        private static readonly string _hurtSound = GlobalHelper.CompanionHitOne;
        private static readonly string _dieSound = GlobalHelper.CompanionDieOne;

        public Knight(Race.RaceType rType, bool isPlayer) : base(rType, EntityClass.Knight, isPlayer, _hurtSound, _dieSound)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Knight");

            GenerateStartingEquipment(EntityClass.Knight, _startingEquipmentTable);
        }
    }
}
