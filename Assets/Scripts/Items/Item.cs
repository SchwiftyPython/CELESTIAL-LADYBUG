﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Abilities;
using Assets.Scripts.Entities;
using Assets.Scripts.Items.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Items
{
    public class Item : IModifierProvider
    {
        [SerializeField] private string _itemId;

        [SerializeField] protected ItemType ItemType;

        public Item(ItemType itemType)
        {
            ItemType = itemType;
            _itemId = Guid.NewGuid().ToString();
        }

        public Sprite GetIcon()
        {
            return ItemType.Sprite;
        }

        public string GetItemId()
        {
            return _itemId;
        }

        public bool IsStackable()
        {
            return ItemType.Stackable;
        }

        public string GetDisplayName()
        {
            return ItemType.Name;
        }

        public string GetDescription()
        {
            return ItemType.Description;
        }

        public ItemGroup GetItemGroup()
        {
            return ItemType.Group;
        }

        public string GetItemGroupForDisplay()
        {
            return GlobalHelper.GetEnumDescription(ItemType.Group);
        }

        public string GetValue()
        {
            return ItemType.Price.ToString();
        }

        public Attack GetMeleeAttack()
        {
            return ItemType.Melee;
        }

        public Attack GetRangedAttack()
        {
            return ItemType.Ranged;
        }

        public Defense GetDefense()
        {
            return ItemType.Defense;
        }

        public int GetToughness()
        {
            if (ItemType.Defense == null)
            {
                return 0;
            }

            return ItemType.Defense.Toughness;
        }

        private int GetDodgeMod()
        {
            if (ItemType.Defense == null)
            {
                return 0;
            }

            return ItemType.Defense.DodgeMod;
        }

        public string GetDodgeModForDisplay()
        {
            return GetDodgeMod().ToString();
        }

        public List<Ability> GetAbilities(Entity abilityOwner)
        {
            var abilities = new List<Ability>();

            if (ItemType.Abilities == null || ItemType.Abilities.Count < 1)
            {
                return abilities;
            }

            foreach (var abilityName in ItemType.Abilities)
            {
                var abilityStore = Object.FindObjectOfType<AbilityStore>();
                var ability = abilityStore.GetAbilityByName(abilityName, abilityOwner);

                if (ability == null)
                {
                    continue;
                }

                abilities.Add(ability);
            }

            return abilities;
        }

        public bool IsTwoHanded()
        {
            return ItemType.TwoHanded;
        }

        public bool IsWeapon()
        {
            if ((ItemType.Ranged == null || ItemType.Ranged.MinDamage == 0 && ItemType.Ranged.MaxDamage == 0) &&
                (ItemType.Melee == null || ItemType.Melee.MinDamage == 0 && ItemType.Melee.MaxDamage == 0))
            {
                return false;
            }

            return true;
        }

        public IEnumerable<float> GetAdditiveModifiers(Enum stat)
        {
            var total = 0;

            try
            {
                if (!Enum.IsDefined(typeof(EntitySkillTypes), stat))
                {
                    total += 0;
                }

                if (!Enum.TryParse<EntitySkillTypes>(stat.ToString(), out var statType))
                {
                    total += 0;
                }

                if (statType == EntitySkillTypes.Dodge)
                {
                    total += GetDodgeMod();
                }
            }
            catch
            {
                total += 0;
            }

            yield return total;
        }

        public IEnumerable<float> GetPercentageModifiers(Enum stat)
        {
            yield return 0f;
        }
    }
}
