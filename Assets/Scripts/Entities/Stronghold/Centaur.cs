using System.Collections.Generic;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Stronghold
{
    public class Centaur : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>>
        {
            {EquipLocation.Weapon, new List<string> {"Crossbow", "Light Crossbow", "Heavy Crossbow"}},
            {EquipLocation.Helmet, new List<string> {"Leather Helmet", "Bycocket", null}},
            {EquipLocation.Boots, new List<string> {"Leather Boots", "Worn Leather Boots"}},
            {EquipLocation.Body, new List<string> {"Leather Armor", "Worn Mail Shirt"}}
        };

        public Centaur(bool isPlayer) : base(Race.RaceType.Beast, EntityClass.Crossbowman, isPlayer) //todo need archer class and bows
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Centaur");

            GenerateStartingEquipment(EntityClass.Crossbowman, _startingEquipmentTable);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.companionHurt;
            DieSound = audioStore.companionDie;
            AttackSound = audioStore.bowAttack;
        }
    }
}
