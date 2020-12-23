using System.Collections.Generic;
using Assets.Scripts.Encounters;

namespace Assets.Scripts.Decks
{
    public class EncounterDeck : Deck<Encounter>
    {
        private const int CommonCap = 5;
        private const int UncommonCap = 2;
        private const int RareCap = 1;

        private const int NormalCap = 5;
        private const int CrossroadsCap = 2;
        private const int TradingCap = 1;
        private const int CampingCap = 2;
        private const int CombatCap = 3;
        private const int ContinuityCap = 1;

        private RarityCapper _rarityCapper;
        private EncounterTypeCapper _encounterTypeCapper;

        public override List<Encounter> Cards { get; set; }

        public EncounterDeck()
        {
            CardIndex = 0;
            Build(EncounterStore.Instance.GetAllNonTriggeredEncounters()); 
            Shuffle();
            _rarityCapper = new RarityCapper(CommonCap, UncommonCap, RareCap);
            _encounterTypeCapper = new EncounterTypeCapper(NormalCap, CrossroadsCap, TradingCap, CampingCap, CombatCap, ContinuityCap);
        }

        public override Encounter Draw()
        {
            var maxTries = 8;
            var validCard = false;
            var numTries = 0;

            Encounter card = null;
            while (!validCard && numTries < maxTries)
            {
                numTries++;

                card = Cards[CardIndex];

                if (!_rarityCapper.IsCapped(card.Rarity) && !_encounterTypeCapper.IsCapped(card.EncounterType))
                {
                    validCard = true;
                }

                if (CardIndex >= Size - 1)
                {
                    Shuffle();
                    CardIndex = 0;
                }
                else
                {
                    CardIndex++;
                }
            }

            if (card != null)
            {
                _rarityCapper.RecordCard(card.Rarity);
                _encounterTypeCapper.RecordCard(card.EncounterType);
                return card;
            }

            return null;
        }
    }
}
