using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;

public class RarityCapper
{
    private readonly int _numCardsToRecord;

    private readonly Dictionary<Rarity, int> _rarityCaps;

    public Queue<Rarity> RecordedCards;

    public RarityCapper(int numCardsToRecord, int commonCap, int uncommonCap, int rareCap)
    {
        RecordedCards = new Queue<Rarity>();

        _numCardsToRecord = numCardsToRecord;

        _rarityCaps = new Dictionary<Rarity, int>
        {
            { Rarity.Common, commonCap },
            { Rarity.Uncommon, uncommonCap },
            { Rarity.Rare, rareCap }
        };
    }

    public bool IsCapped(Rarity rarity)
    {
        var rarityCount = RecordedCards.Count(card => card == rarity);

        return rarityCount >= _rarityCaps[rarity];
    }

    public void RecordCardRarity(Rarity rarity)
    {
        if (RecordedCards.Count >= _numCardsToRecord)
        {
            RecordedCards.Dequeue();
        }
        RecordedCards.Enqueue(rarity);
    }
}
