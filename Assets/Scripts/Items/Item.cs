﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Items.Components;
using UnityEngine;

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

        public IEnumerable<float> GetAdditiveModifiers<T>(T stat)
        {
            if ((EntitySkillTypes)(object)stat == EntitySkillTypes.Dodge)
            {
                yield return GetDodgeMod();
            }
        }

        public IEnumerable<float> GetPercentageModifiers<T>(T stat)
        {
            yield return 0f;
        }
    }
}
