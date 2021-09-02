using System.Collections.Generic;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Companions
{
    public class ManAtArms : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>>
        {
            {EquipLocation.Weapon, new List<string> { "Short Sword", "Sword", "Broad Sword"}},
            {EquipLocation.Helmet, new List<string> {"Leather Helmet", null}},
            {EquipLocation.Boots, new List<string> {"Leather Boots", "Worn Leather Boots"}},
            {EquipLocation.Gloves, new List<string> {"Leather Gloves", "Metal Bracers", null}},
            {EquipLocation.Body, new List<string> {"Worn Mail Shirt", "Mail Shirt"}},
            {EquipLocation.Shield, new List<string> {"Knight's Shield", "Wooden Shield", "Heater Shield"}}
        };

        public ManAtArms(Race.RaceType rType, bool isPlayer) : base(rType, EntityClass.ManAtArms, isPlayer)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("ManAtArms");

            GenerateStartingEquipment(EntityClass.ManAtArms, _startingEquipmentTable);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.companionHurt;
            DieSound = audioStore.companionDie;
            AttackSound = audioStore.genericAttack;

            var spriteStore = Object.FindObjectOfType<SpriteStore>();

            IdleSkinSwap = spriteStore.ManAtArmsIdleSwap;
            AttackSkinSwap = spriteStore.ManAtArmsAttackSwap;
        }
    }
}
