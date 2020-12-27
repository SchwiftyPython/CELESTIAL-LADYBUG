using System.Collections.Generic;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Encounters
{
    public class Reward 
    {
        public Dictionary<Entity, KeyValuePair<object, int>> EntityGains;
        public Dictionary<object, int> PartyGains;
        public List<Effect> Effects;

        public Reward(Dictionary<Entity, KeyValuePair<object, int>> entityGains, Dictionary<object, int> partyGains, List<Effect> effects)
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
    }
}
