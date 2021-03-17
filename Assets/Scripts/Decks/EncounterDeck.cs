using System.Collections.Generic;
using Assets.Scripts.Encounters;
using UnityEngine;

namespace Assets.Scripts.Decks
{
    public class EncounterDeck : Deck<Encounter>
    {
        private const int CommonCap = 5;
        private const int UncommonCap = 2;
        private const int RareCap = 1;

        public override Queue<Encounter> Cards { get; set; }

        public EncounterDeck(List<Encounter> cardPool, int deckSize)
        {
            CardIndex = 0;
            Build(cardPool, deckSize, new RarityCapper(CommonCap, UncommonCap, RareCap));
            Shuffle();
        }

        public sealed override void Build(List<Encounter> cardPool, int deckSize, RarityCapper capper)
        {
            Cards = new Queue<Encounter>();
            Size = deckSize;

            var usedIndexes = new List<int>();

            while (Cards.Count < Size)
            {
                const int maxTries = 4;
                var validCard = false;
                var numTries = 0;

                var index = Random.Range(0, cardPool.Count);

                usedIndexes.Add(index);

                Encounter card = null;

                while (!validCard && numTries < maxTries)
                {
                    numTries++;

                    if (usedIndexes.Contains(index))
                    {
                        continue;
                    }

                    card = cardPool[index];

                    if (!capper.IsCapped(card.Rarity))
                    {
                        validCard = true;
                    }

                    index = Random.Range(0, cardPool.Count);
                }

                if (card == null)
                {
                    card = cardPool[Random.Range(0, cardPool.Count)];
                }

                AddCard(card);
            }
        }

        public override Encounter Draw()
        {
            if (Cards == null || Cards.Count < 1)
            {
                return null;
            }

            Size--;

            return Cards.Dequeue();
        }
    }
}
