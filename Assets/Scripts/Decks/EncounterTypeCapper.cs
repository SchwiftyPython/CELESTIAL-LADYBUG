using System.Collections.Generic;
using Assets.Scripts.Encounters;

namespace Assets.Scripts.Decks
{
    public class EncounterTypeCapper : Capper<EncounterType>
    {
        public sealed override Dictionary<EncounterType, int> Caps { get; set; }

        public EncounterTypeCapper(int normalCap, int crossroadsCap, int tradingCap, int campingCap, int combatCap, int continuityCap)
        {
            RecordedCards = new Queue<EncounterType>();

            NumCardsToRecord = normalCap + crossroadsCap + tradingCap + campingCap + combatCap + continuityCap;

            Caps = new Dictionary<EncounterType, int>
            {
                {EncounterType.Normal, normalCap},
                {EncounterType.Crossroads, crossroadsCap},
                {EncounterType.Trading, tradingCap},
                {EncounterType.Camping, campingCap},
                {EncounterType.Combat, combatCap},
                {EncounterType.Continuity, continuityCap}
            };
        }
    }
}
