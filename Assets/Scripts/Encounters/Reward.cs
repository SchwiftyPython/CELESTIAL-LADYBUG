using System.Collections.Generic;
using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using Assets.Scripts.Items;

namespace Assets.Scripts.Encounters
{
    public class Reward 
    {
        public Dictionary<Entity, List<KeyValuePair<EntityStatTypes, int>>> EntityStatGains;
        public Dictionary<Entity, List<KeyValuePair<EntityAttributeTypes, int>>> EntityAttributeGains;
        public Dictionary<Entity, List<KeyValuePair<EntitySkillTypes, int>>> EntitySkillGains;
        public Dictionary<PartySupplyTypes, int> PartyGains;
        public List<Entity> PartyAdditions;
        public List<EquipableItem> InventoryGains;
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

        public void AddEntityGain(Entity targetEntity, EntitySkillTypes skillType, int amountGained)
        {
            if (EntitySkillGains == null)
            {
                EntitySkillGains = new Dictionary<Entity, List<KeyValuePair<EntitySkillTypes, int>>>();
            }

            var gain = new KeyValuePair<EntitySkillTypes, int>(skillType, amountGained);

            if (!EntitySkillGains.ContainsKey(targetEntity))
            {
                EntitySkillGains.Add(targetEntity, new List<KeyValuePair<EntitySkillTypes, int>> { gain });
            }
            else
            {
                EntitySkillGains[targetEntity].Add(gain);
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

        public void AddToParty(Entity companion)
        {
            if (PartyAdditions == null)
            {
                PartyAdditions = new List<Entity>();
            }

            PartyAdditions.Add(companion);
        }

        public void AddToInventory(EquipableItem item)
        {
            if (InventoryGains == null)
            {
                InventoryGains = new List<EquipableItem>();
            }

            InventoryGains.Add(item);
        }

        public void EveryoneGain(Party party, EntityStatTypes statType, int amountGained)
        {
            AddEntityGain(party.Derpus, statType, amountGained);

            foreach (var companion in party.GetCompanions())
            {
                AddEntityGain(companion, statType, amountGained);
            }
        }

        public void EveryoneGain(Party party, EntityAttributeTypes attributeType, int amountGained)
        {
            AddEntityGain(party.Derpus, attributeType, amountGained);

            foreach (var companion in party.GetCompanions())
            {
                AddEntityGain(companion, attributeType, amountGained);
            }
        }

        public void EveryoneGain(Party party, EntitySkillTypes skillType, int amountGained)
        {
            AddEntityGain(party.Derpus, skillType, amountGained);

            foreach (var companion in party.GetCompanions())
            {
                AddEntityGain(companion, skillType, amountGained);
            }
        }
    }
}
