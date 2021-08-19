using System.Collections.Generic;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Companions
{
    public class Spearman : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>>
        {
            {EquipLocation.Weapon, new List<string> { "Spear", "Lance" }},
            {EquipLocation.Helmet, new List<string> {"Leather Helmet", null}},
            {EquipLocation.Boots, new List<string> {"Leather Boots", "Worn Leather Boots"}},
            {EquipLocation.Gloves, new List<string> {"Leather Gloves", "Metal Bracers", null}},
            {EquipLocation.Body, new List<string> {"Worn Mail Shirt", "Mail Shirt"}}
        };

        public Spearman(Race.RaceType rType, bool isPlayer) : base(rType, EntityClass.Spearman, isPlayer)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Spearman");
            
            GenerateStartingEquipment(EntityClass.Spearman, _startingEquipmentTable);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.companionHurt;
            DieSound = audioStore.companionDie;
            AttackSound = audioStore.genericAttack;
        }
    }
}
