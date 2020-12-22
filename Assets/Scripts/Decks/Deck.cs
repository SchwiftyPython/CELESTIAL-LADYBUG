﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Decks
{
    public abstract class Deck<T>
    {
        public int Size;

        public int CardIndex;

        public abstract List<T> Cards { get; set; }

        public abstract T Draw();

        public void Build(List<T> cards)
        {
            Cards = new List<T>(cards);
            Size = Cards.Count;
        }

        public void Shuffle()
        {
            for (var i = Cards.Count - 1; i > 0; i--)
            {
                var n = Random.Range(0, i + 1);
                var temp = Cards[i];
                Cards[i] = Cards[n];
                Cards[n] = temp;
            }
        }
    }
}
