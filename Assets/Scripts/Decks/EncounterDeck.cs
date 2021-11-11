using System.Collections.Generic;
using Assets.Scripts.Encounters;
using Assets.Scripts.Saving;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Decks
{
    public sealed class EncounterDeck : Deck<Encounter>, ISaveable
    {
        private const int CommonCap = 5;
        private const int UncommonCap = 2;
        private const int RareCap = 1;

        public override Queue<Encounter> Cards { get; set; }

        public EncounterDeck(List<Encounter> cardPool, int deckSize, List<Encounter> usedEncounters)
        {
            CardIndex = 0;
            Build(cardPool, deckSize, new RarityCapper(CommonCap, UncommonCap, RareCap), usedEncounters);
            Shuffle();
        }

        public override void Build(List<Encounter> cardPool, int deckSize, RarityCapper capper, List<Encounter> usedEncounters)
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();

            var currentBiome = travelManager.CurrentBiome;

            Cards = new Queue<Encounter>();
            Size = deckSize;

            var usedIndexes = new List<int>();

            while (Cards.Count < Size)
            {
                const int maxTries = 4;
                var validCard = false;
                var numTries = 0;

                Encounter card = null;

                while (!validCard && numTries < maxTries)
                {
                    numTries++;

                    var index = Random.Range(0, cardPool.Count);

                    if (usedIndexes.Contains(index))
                    {
                        continue;
                    }

                    usedIndexes.Add(index);

                    card = cardPool[index];

                    if (usedEncounters.Contains(card))
                    {
                        continue;
                    }

                    if (!capper.IsCapped(card.Rarity) && card.ValidBiome(currentBiome))
                    {
                        validCard = true;
                    }
                }

                if (!validCard)
                {
                    usedEncounters.Clear();
                }

                AddCard(card);

                usedEncounters.Add(card);
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

        public struct EncounterDeckDto
        {
            public int Size;
        }

        public object CaptureState()
        {
            var dto = new EncounterDeckDto
            {
                Size = Cards.Count
            };

            return dto;
        }

        public void RestoreState(object state)
        {
            throw new System.NotImplementedException();
        }
    }
}
