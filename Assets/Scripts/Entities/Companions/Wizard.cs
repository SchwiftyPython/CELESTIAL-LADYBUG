using System.Collections.Generic;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Companions
{
    public class Wizard : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>>
        {
            {EquipLocation.Book, new List<string> { null }},
            {EquipLocation.Helmet, new List<string> {"Leather Headband", null}},
            {EquipLocation.Boots, new List<string> {null}}, //todo sandals or something
            {EquipLocation.Body, new List<string> {"Robe", "Wizard's Robe"}}
        };

        public Wizard(Race.RaceType rType, bool isPlayer) : base(rType, EntityClass.Wizard, isPlayer)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Wizard");

            GenerateStartingEquipment(EntityClass.Wizard, _startingEquipmentTable);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.companionHurt;
            DieSound = audioStore.companionDie;
        }
    }
}
