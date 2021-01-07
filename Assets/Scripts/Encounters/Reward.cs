using System.Collections.Generic;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Encounters
{
    public class Reward 
    {
        public Dictionary<Entity, List<KeyValuePair<EntityStatTypes, int>>> EntityGains;
        public Dictionary<PartySupplyTypes, int> PartyGains;
        public List<Effect> Effects;

        /*public Reward(Dictionary<Entity, KeyValuePair<object, int>> entityGains, Dictionary<object, int> partyGains, List<Effect> effects)
        {
            EntityGains = entityGains;
            PartyGains = partyGains;
            Effects = effects;
        }

        public Reward(Dictionary<object, int> partyGains)
        {
            PartyGains = partyGains;
        }

        public Reward(Dictionary<Entity, KeyValuePair<object, int>> entityGains)
        {
            EntityGains = entityGains;
        }

        public Reward(Dictionary<Entity, KeyValuePair<object, int>> entityGains, Dictionary<object, int> partyGains)
        {
            EntityGains = entityGains;
            PartyGains = partyGains;
        }*/

        public void AddEntityGain(Entity targetEntity, EntityStatTypes statType, int amountGained)
        {
            if (EntityGains == null)
            {
                EntityGains = new Dictionary<Entity, List<KeyValuePair<EntityStatTypes, int>>>();
            }

            var gain = new KeyValuePair<EntityStatTypes, int>(statType, amountGained);

            if (!EntityGains.ContainsKey(targetEntity))
            {
                EntityGains.Add(targetEntity, new List<KeyValuePair<EntityStatTypes, int>> { gain });
            }
            else
            {
                EntityGains[targetEntity].Add(gain);
            }
        }

        public void AddPartyGain(PartySupplyTypes supplyType, int amountGained)
        {
            if (PartyGains == null)
            {
                PartyGains = new Dictionary<PartySupplyTypes, int>();
            }

            if (!PartyGains.ContainsKey(supplyType))
            {
                PartyGains.Add(supplyType, amountGained);
            }
            else
            {
                PartyGains[supplyType] += amountGained;
            }
        }
    }
}
