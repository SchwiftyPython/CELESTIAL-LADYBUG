using System.Collections.Generic;
using Assets.Scripts.Abilities;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class Vampire : Entity
    {
        private readonly Dictionary<EquipLocation, List<string>> _startingEquipmentTable = new Dictionary<EquipLocation, List<string>>
        {
            {EquipLocation.Book, new List<string> { null }},
            {EquipLocation.Helmet, new List<string> {"Leather Headband", null}},
            {EquipLocation.Boots, new List<string> {null}}, //todo sandals or something
            {EquipLocation.Body, new List<string> {"Robe", "Wizard's Robe"}}
        };

        public Vampire() : base(Race.RaceType.Undead, EntityClass.Wizard, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Vampire");

            //todo need some spells boy-o

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var vBite = abilityStore.GetAbilityByName("vampire bite", this);

            Abilities.Add(vBite.GetType(), vBite);

            if (Abilities.ContainsKey(typeof(Intimidate)))
            {
                return;
            }

            var intimidate = abilityStore.GetAbilityByName("intimidate", this);

            Abilities.Add(intimidate.GetType(), intimidate);

            GenerateStartingEquipment(EntityClass.Wizard, _startingEquipmentTable);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.monsterHurt;
            DieSound = audioStore.monsterDie;
            AttackSound = audioStore.genericAttack;
        }
    }
}
