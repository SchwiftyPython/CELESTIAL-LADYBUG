using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class DeathKnight : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>>
        {
            {EquipLocation.Weapon, new List<string> { "Broad Sword", "Scimitar"}},
            {EquipLocation.Helmet, new List<string> {"Leather Helmet", "Close Helm"}},
            {EquipLocation.Boots, new List<string> {"Leather Boots", "Worn Leather Boots"}},
            {EquipLocation.Gloves, new List<string> {"Leather Gloves", "Metal Bracers", "Plate Gauntlets", null}},
            {EquipLocation.Body, new List<string> {"Plate Armor", "Mail Shirt"}}
        };

        public DeathKnight() : base(Race.RaceType.Undead, EntityClass.BattleMage, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("DeathKnight");

            GenerateStartingEquipment(EntityClass.BattleMage, _startingEquipmentTable);

            //todo need some mount specific abilities
        }
    }
}
