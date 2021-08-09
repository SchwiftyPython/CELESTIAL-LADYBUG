using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class Lich : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>>
        {
            {EquipLocation.Book, new List<string> { null }},
            {EquipLocation.Helmet, new List<string> {"Leather Headband", null}},
            {EquipLocation.Boots, new List<string> {null}}, //todo sandals or something
            {EquipLocation.Body, new List<string> {"Robe", "Wizard's Robe"}}
        };

        private static readonly string _hurtSound = GlobalHelper.MonsterHitOne;
        private static readonly string _dieSound = GlobalHelper.MonsterDieOne;

        public Lich() : base(Race.RaceType.Undead, EntityClass.Wizard, false, _hurtSound, _dieSound)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Lich");

            GenerateStartingEquipment(EntityClass.Wizard, _startingEquipmentTable);
        }
    }
}
