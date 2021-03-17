using System.Collections.Generic;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Encounters
{
    public class Reward 
    {
        public Dictionary<Entity, List<KeyValuePair<EntityStatTypes, int>>> EntityStatGains;
        public Dictionary<Entity, List<KeyValuePair<EntityAttributeTypes, int>>> EntityAttributeGains;
        public Dictionary<PartySupplyTypes, int> PartyGains;
        public List<Effect> Effects;

        public void AddEntityGain(Entity targetEntity, EntityStatTypes statType, int amountGained)
        {
            if (EntityStatGains == null)
            {
                EntityStatGains = new Dictionary<Entity, List<KeyValuePair<EntityStatTypes, int>>>();
            }

            var gain = new KeyValuePair<EntityStatTypes, int>(statType, amountGained);

            if (!EntityStatGains.ContainsKey(targetEntity))
            {
                EntityStatGains.Add(targetEntity, new List<KeyValuePair<EntityStatTypes, int>> { gain });
            }
            else
            {
                EntityStatGains[targetEntity].Add(gain);
            }
        }

        public void AddEntityGain(Entity targetEntity, EntityAttributeTypes attributeType, int amountGained)
        {
            if (EntityAttributeGains == null)
            {
                EntityAttributeGains = new Dictionary<Entity, List<KeyValuePair<EntityAttributeTypes, int>>>();
            }

            var gain = new KeyValuePair<EntityAttributeTypes, int>(attributeType, amountGained);

            if (!EntityAttributeGains.ContainsKey(targetEntity))
            {
                EntityAttributeGains.Add(targetEntity, new List<KeyValuePair<EntityAttributeTypes, int>> { gain });
            }
            else
            {
                EntityAttributeGains[targetEntity].Add(gain);
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
