using System;
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

        public Item()
        {
        }

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

        public int GetValue()
        {
            return ItemType.Price;
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

        public int GetDodgeMod()
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

        public List<string> GetAbilityNames()
        {
            return ItemType.Abilities;
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

        public string GetItemSound()
        {
            //todo either add to itemtypes or just give sound to groups like sword spear etc
            return string.Empty;
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

        public float GetAdditiveModifiers(Enum stat)
        {
            var total = 0;

            try
            {
                if (!stat.GetType().Name.Equals(nameof(EntitySkillTypes)))
                {
                    total += 0;
                }

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

            return total;
        }

        public float GetPercentageModifiers(Enum stat)
        {
            return 0f;
        }
    }
}
