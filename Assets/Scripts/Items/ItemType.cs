﻿using System.Collections.Generic;
using Assets.Scripts.Items.Components;

namespace Assets.Scripts.Items
{
    //todo maybe make constructor for items here so we can define an Equippable item, armor, weapon, etc based on what fields are populated. Like the Breed.NewMonster example
    public class ItemType
    {
        public string Name;
        public string Parent;

        private Attack _melee;
        public Attack Melee
        {
            get => _melee;
            set
            {
                if (!string.IsNullOrEmpty(Parent) && value.MinDamage <= 0 && value.MaxDamage <= 0)
                {
                    _melee = ItemStore.Instance.GetItemTypeByName(Parent).Melee;
                }
                else
                {
                    _melee = value;
                }
            }
        }

        private Attack _ranged;

        public Attack Ranged
        {
            get => _ranged;
            set
            {
                if (!string.IsNullOrEmpty(Parent) && value.MinDamage <= 0 && value.MaxDamage <= 0)
                {
                    _ranged = ItemStore.Instance.GetItemTypeByName(Parent).Ranged;
                }
                else
                {
                    _ranged = value;
                }
            }
        }

        private Defense _defense;
        public Defense Defense
        {
            get => _defense;
            set
            {
                if (!string.IsNullOrEmpty(Parent) && value.Toughness <= 0 && value.DodgeMod <= 0)
                {
                    _defense = ItemStore.Instance.GetItemTypeByName(Parent).Defense;
                }
                else
                {
                    _defense = value;
                }
            }
        }

        //todo convert to List<Ability>
        private List<string> _abilities;
        public List<string> Abilities
        {
            get => _abilities;
            set
            {
                if (!string.IsNullOrEmpty(Parent) && value == null || value.Count <= 0)
                {
                    _abilities = ItemStore.Instance.GetItemTypeByName(Parent)?.Abilities;
                }
                else
                {
                    _abilities = value;
                }
            }
        }

        //todo convert to Sprite
        private string _sprite;
        public string Sprite
        {
            get => _sprite;
            set
            {
                if (!string.IsNullOrEmpty(Parent) && string.IsNullOrEmpty(value))
                {
                    _sprite = ItemStore.Instance.GetItemTypeByName(Parent).Sprite;
                }
                else
                {
                    _sprite = value;
                }
            }
        }

        private EquipLocation? _slot;
        public EquipLocation? Slot
        {
            get => _slot;
            set
            {
                if (!string.IsNullOrEmpty(Parent) && value == null)
                {
                    _slot = ItemStore.Instance.GetItemTypeByName(Parent).Slot;
                }
                else
                {
                    _slot = value;
                }
            }
        }
    }
}
