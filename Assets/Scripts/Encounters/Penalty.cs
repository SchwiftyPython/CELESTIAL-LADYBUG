﻿using System.Collections.Generic;
using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using GoRogue;

namespace Assets.Scripts.Encounters
{
    public class Penalty
    {
        public Dictionary<Entity, List<KeyValuePair<EntityStatTypes, int>>> EntityStatLosses;
        public Dictionary<Entity, List<KeyValuePair<EntityAttributeTypes, int>>> EntityAttributeLosses;
        public Dictionary<PartySupplyTypes, int> PartyLosses;
        public List<Effect> Effects;

        public void AddEntityLoss(Entity targetEntity, EntityStatTypes statType, int amountLost)
        {
            if (EntityStatLosses == null)
            {
                EntityStatLosses = new Dictionary<Entity, List<KeyValuePair<EntityStatTypes, int>>>();
            }

            var loss = new KeyValuePair<EntityStatTypes, int>(statType, amountLost);

            if (!EntityStatLosses.ContainsKey(targetEntity))
            {
                EntityStatLosses.Add(targetEntity, new List<KeyValuePair<EntityStatTypes, int>>{loss});
            }
            else
            {
                EntityStatLosses[targetEntity].Add(loss);
            }
        }

        public void AddEntityLoss(Entity targetEntity, EntityAttributeTypes attributeType, int amountLost)
        {
            if (EntityAttributeLosses == null)
            {
                EntityAttributeLosses = new Dictionary<Entity, List<KeyValuePair<EntityAttributeTypes, int>>>();
            }

            var loss = new KeyValuePair<EntityAttributeTypes, int>(attributeType, amountLost);

            if (!EntityAttributeLosses.ContainsKey(targetEntity))
            {
                EntityAttributeLosses.Add(targetEntity, new List<KeyValuePair<EntityAttributeTypes, int>> { loss });
            }
            else
            {
                EntityAttributeLosses[targetEntity].Add(loss);
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
