using System.Collections.Generic;

namespace Assets.Scripts.Decks
{
    public class RarityCapper: Capper<Rarity>
    {
        public sealed override Dictionary<Rarity, int> Caps { get; set; }

        public RarityCapper(int commonCap, int uncommonCap, int rareCap)
        {
            RecordedCards = new Queue<Rarity>();

            NumCardsToRecord = commonCap + uncommonCap + rareCap;

            Caps = new Dictionary<Rarity, int>
            {
                { Rarity.Common, commonCap },
                { Rarity.Uncommon, uncommonCap },
                { Rarity.Rare, rareCap }
            };
        }
    }
}
