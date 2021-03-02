using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Decks
{
    public abstract class Deck<T>
    {
        public int Size;

        public int CardIndex;

        public abstract Queue<T> Cards { get; set; }

        public abstract T Draw();

        public virtual void Build(List<T> cards)
        {
            Cards = new Queue<T>(cards);
            Size = Cards.Count;
        }

        public virtual void Build(List<T> cardPool, int deckSize, RarityCapper capper) {}

        public void Shuffle()
        {
            var cardList = Cards.ToList();

            for (var i = Cards.Count - 1; i > 0; i--)
            {
                var n = Random.Range(0, i + 1);
                var temp = cardList[i];
                cardList[i] = cardList[n];
                cardList[n] = temp;
            }

            Cards = new Queue<T>(cardList);
        }
    }
}
