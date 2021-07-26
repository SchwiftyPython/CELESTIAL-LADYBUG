using System.Collections.Generic;
using Assets.Scripts.Abilities;
using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class Zombie : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>>
        {
            {EquipLocation.Weapon, new List<string> { "Short Sword", "Sword", "Broad Sword"}},
            {EquipLocation.Helmet, new List<string> {"Leather Helmet", null}},
            {EquipLocation.Boots, new List<string> {"Worn Leather Boots", null}},
            {EquipLocation.Body, new List<string> {"Worn Mail Shirt", null}}
        };

        private static readonly string _hurtSound = "";
        private static readonly string _dieSound = "";

        public Zombie() : base(Race.RaceType.Undead, EntityClass.ManAtArms, false, _hurtSound, _dieSound)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Zombie");

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var eEndurance = abilityStore.GetAbilityByName("endangered endurance", this);

            Abilities.Add(eEndurance.GetType(), eEndurance);

            GenerateStartingEquipment(EntityClass.ManAtArms, _startingEquipmentTable);
        }
    }
}
