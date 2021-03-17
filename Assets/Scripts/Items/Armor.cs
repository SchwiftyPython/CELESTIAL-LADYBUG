﻿namespace Assets.Scripts.Items
{
    public class Armor : Item
    {
        //todo going to try minecraft damage protection
        public int Toughness { get; private set; }

        public Armor(string itemName, ItemType type, int toughness) : base(itemName, type)
        {
            Toughness = toughness;
        }
    }
}
