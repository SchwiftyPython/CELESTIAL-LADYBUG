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
            Build(); //todo get from EncounterStore
            Shuffle();
            _rarityCapper = new RarityCapper(CommonCap, UncommonCap, RareCap);
            _encounterTypeCapper = new EncounterTypeCapper(NormalCap, CrossroadsCap, TradingCap, CampingCap, CombatCap, ContinuityCap);
        }

        public override Encounter Draw()
        {
            throw new System.NotImplementedException();
        }
    }
}
