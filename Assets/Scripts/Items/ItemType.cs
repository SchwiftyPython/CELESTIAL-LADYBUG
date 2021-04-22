using System.Collections.Generic;
using Assets.Scripts.Items.Components;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemType
    {
        public string Name;
        public string Description;
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

        private int _range;
        public int Range
        {
            get => _range;
            set
            {
                if (!string.IsNullOrEmpty(Parent) && value <= 0)
                {
                    _range = ItemStore.Instance.GetItemTypeByName(Parent).Range;
                }
                else
                {
                    _range = value;
                }
            }
        }

        //todo convert to Sprite
        private Sprite _sprite;
        public Sprite Sprite
        {
            get => _sprite;
            set
            {
                if (!string.IsNullOrEmpty(Parent) && value == null)
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

        public bool Stackable;

        public int Price;

        public bool TwoHanded;

        public ItemGroup Group;

        public Item NewItem()
        {
            if (!IsEquipable())
            {
                return new Item(this);
            }

            return new EquipableItem(this);
        }

        public bool IsEquipable()
        {
            return Slot != null;
        }
    }
}
