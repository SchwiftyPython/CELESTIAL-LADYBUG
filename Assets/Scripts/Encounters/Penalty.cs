using System.Collections.Generic;
using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using GoRogue;

namespace Assets.Scripts.Encounters
{
    public class Penalty
    {
        public Dictionary<Entity, List<KeyValuePair<EntityStatTypes, int>>> EntityLosses;
        public Dictionary<PartySupplyTypes, int> PartyLosses;
        public List<Effect> Effects;

        public void AddEntityLoss(Entity targetEntity, EntityStatTypes statType, int amountLost)
        {
            if (EntityLosses == null)
            {
                EntityLosses = new Dictionary<Entity, List<KeyValuePair<EntityStatTypes, int>>>();
            }

            var loss = new KeyValuePair<EntityStatTypes, int>(statType, amountLost);

            if (!EntityLosses.ContainsKey(targetEntity))
            {
                EntityLosses.Add(targetEntity, new List<KeyValuePair<EntityStatTypes, int>>{loss});
            }
            else
            {
                EntityLosses[targetEntity].Add(loss);
            }
        }

        public void AddPartyLoss(PartySupplyTypes supplyType, int amountLost)
        {
            if (PartyLosses == null)
            {
                PartyLosses = new Dictionary<PartySupplyTypes, int>();
            }

            if (!PartyLosses.ContainsKey(supplyType))
            {
                PartyLosses.Add(supplyType, amountLost);
            }
            else
            {
                PartyLosses[supplyType] += amountLost;
            }
        }
    }
}
