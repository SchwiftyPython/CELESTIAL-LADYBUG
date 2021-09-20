using System.Collections.Generic;
using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using GoRogue;

namespace Assets.Scripts.Encounters
{
    public class Penalty
    {
        public Dictionary<Entity, List<KeyValuePair<EntityStatTypes, int>>> EntityStatLosses;
        public Dictionary<Entity, List<KeyValuePair<EntityAttributeTypes, int>>> EntityAttributeLosses;
        public Dictionary<Entity, List<KeyValuePair<EntitySkillTypes, int>>> EntitySkillLosses;
        public Dictionary<PartySupplyTypes, int> PartyLosses;
        public List<Entity> PartyRemovals;
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

        public void AddEntityLoss(Entity targetEntity, EntitySkillTypes skillType, int amountLost)
        {
            if (EntitySkillLosses == null)
            {
                EntitySkillLosses = new Dictionary<Entity, List<KeyValuePair<EntitySkillTypes, int>>>();
            }

            var loss = new KeyValuePair<EntitySkillTypes, int>(skillType, amountLost);

            if (!EntitySkillLosses.ContainsKey(targetEntity))
            {
                EntitySkillLosses.Add(targetEntity, new List<KeyValuePair<EntitySkillTypes, int>> { loss });
            }
            else
            {
                EntitySkillLosses[targetEntity].Add(loss);
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

        public void RemoveFromParty(Entity companion)
        {
            if (PartyRemovals == null)
            {
                PartyRemovals = new List<Entity>();
            }

            PartyRemovals.Add(companion);
        }

        public void EveryoneLoss(Party party, EntityStatTypes statType, int amountLost)
        {
            AddEntityLoss(party.Derpus, statType, amountLost);

            foreach (var companion in party.GetCompanions())
            {
                AddEntityLoss(companion, statType, amountLost);
            }
        }
    }
}
