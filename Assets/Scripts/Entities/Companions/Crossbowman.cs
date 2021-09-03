using System.Collections.Generic;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Companions
{
    public sealed class Crossbowman : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>>
        {
            {EquipLocation.Weapon, new List<string> {"Crossbow", "Light Crossbow", "Heavy Crossbow"}},
            {EquipLocation.Helmet, new List<string> {"Leather Helmet", "Bycocket", null}},
            {EquipLocation.Boots, new List<string> {"Leather Boots", "Worn Leather Boots"}},
            {EquipLocation.Body, new List<string> {"Leather Armor", "Worn Mail Shirt"}}
        };

        public Crossbowman(Race.RaceType rType, bool isPlayer) : base(rType, EntityClass.Crossbowman, isPlayer)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Crossbowman");

            GenerateStartingEquipment(EntityClass.Crossbowman, _startingEquipmentTable);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.companionHurt;
            DieSound = audioStore.companionDie;
            AttackSound = audioStore.bowAttack;

            var spriteStore = Object.FindObjectOfType<SpriteStore>();

            IdleSkinSwap = spriteStore.CrossbowmanIdleSwap;
            AttackSkinSwap = spriteStore.CrossbowmanAttackSwap;
            HitSkinSwap = spriteStore.CrossbowmanHitSwap;
            DeadSkinSwap = spriteStore.CrossbowmanDeadSwap;
        }
    }
}
