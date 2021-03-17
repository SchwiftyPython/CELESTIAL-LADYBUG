using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Decks
{
    public abstract class Capper<T> 
    {
        public int NumCardsToRecord;

        public abstract Dictionary<T, int> Caps { get; set; }

        public Queue<T> RecordedCards;

        public bool IsCapped(T capType)
        {
            var capTypeCount = RecordedCards.Count(card => Equals(card, capType));

            return capTypeCount >= Caps[capType];
        }

        public void RecordCard(T capType)
        {
            if (RecordedCards.Count >= NumCardsToRecord)
            {
                RecordedCards.Dequeue();
            }
            RecordedCards.Enqueue(capType);
        }
    }
}
